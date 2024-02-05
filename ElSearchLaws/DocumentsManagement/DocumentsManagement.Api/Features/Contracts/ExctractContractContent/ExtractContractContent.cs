using System.Text.RegularExpressions;
using DocumentsManagement.Api.Features.Common;
using DocumentsManagement.Api.PreProcessor;
using FluentResults;

namespace DocumentsManagement.Api.Features.Contracts.ExctractContractContent;

public class ExtractContractContent: IContentExtractor<Contract>
{
    private readonly ILogger<ExtractContractContent> _logger;

    public ExtractContractContent(ILogger<ExtractContractContent> logger)
    {
        _logger = logger;
    }

    public async Task<Result<Contract>> ExtractContentAsync(IFormFile file, CancellationToken cancellationToken)
    {
          var tempFilePath = Path.GetTempFileName();
        try {
            await using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.ReadWrite)) {
                file.OpenReadStream().Position = 0;
                await file.OpenReadStream().CopyToAsync(fileStream, cancellationToken);
                await fileStream.FlushAsync(cancellationToken);
            }

            var pdfDocument = PdfDocument.FromFile(tempFilePath);
            var text = pdfDocument.ExtractAllText();
            var lines = pdfDocument.Pages[0].Lines;
            var chars = pdfDocument.Pages[0].Characters;
            var metadata = new ContentMetadata(
                pdfDocument.MetaData.Title,
                file.FileName,
                pdfDocument.MetaData.CreationDate,
                pdfDocument.MetaData.Author,
                null
            );

            _logger.LogInformation("Pdf contract text extraction completed for {FileName} with length {@Info}",
                file.FileName,
                new FileInfo(tempFilePath).Length
            );
            var contractDic = ExtractData(text);
            var contractExtracted = ExtractParsedValues(contractDic);

            contractExtracted.Metadata = metadata;

            return contractExtracted;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error extracting pdf law for {FileName}", file.FileName);
            throw;
        } finally {
            // Clean up the temporary file
            if (File.Exists(tempFilePath)) {
                File.Delete(tempFilePath);
            }
        }
    }

    private static Dictionary<string, string> ExtractData(string inputText)
    {
        var tagsDictionary = new Dictionary<string, string>();
        var textLength = inputText.Length;

        // Find the indices to isolate the agency and government segments
        int agencyStartIndex = inputText.IndexOf("Agencija za objavljivanje zakona", StringComparison.Ordinal);

        int govStartIndex = inputText.IndexOf("Uprava za", agencyStartIndex, StringComparison.Ordinal);
        if (govStartIndex == -1) {
            govStartIndex = textLength;
        }
        var phoneMatch = Regex.Match(inputText.Substring(govStartIndex), @"\d{7,}");
        int phoneIndex = phoneMatch.Success
            ? govStartIndex + phoneMatch.Index + phoneMatch.Length - 1
            : textLength;

        // Extract segments from the input text
        var agencySegment = inputText.Substring(agencyStartIndex, govStartIndex - agencyStartIndex);
        var govSegment = inputText.Substring(govStartIndex, phoneIndex - govStartIndex);

        // Define patterns for agency and government data extraction
        var agencyPatterns = new Dictionary<string, string> {
            { "NAZIV_ULICE_AGENCIJE", @"Agencija za objavljivanje zakona\s+(?<value>[^,\r\n]+),\s*\d+" },
            { "BROJ_ZGRADE_AGENCIJE", @"\s*,\s*(?<value>\d+),\s*[^,\r\n]+" },
            { "GRAD_AGENCIJE", @"\d+,\s*(?<value>[^,\r\n]+)" },
            { "EMAIL_AGENCIJE", @"(?<value>\S+@\S+\.\S+)" },
            { "TELEFON_AGENCIJE", @"(?<value>\d{7,})" }
        };

        var govPatterns = new Dictionary<string, string>() {
            { "NAZIV_UPRAVE", @"Uprava za (?<value>[^\r\n]+)" },
            { "NIVO_UPRAVE", @"Nivo uprave: (?<value>\S+)" },
            { "NAZIV_ULICE", @"(?<value>[^,\r\n]+),\s*\d+" },
            { "BROJ_ZGRADE", @"\s*,\s*(?<value>\d+),\s*[^,\r\n]+" },
            { "GRAD", @"\d+,\s*(?<value>[^,\r\n]+)" },
            { "EMAIL", @"(?<value>\S+@\S+\.\S+)" },
            { "TELEFON", @"(?<value>\d{7,})" }
        };

        // Apply agency patterns to the agency segment
        foreach (var pattern in agencyPatterns) {
            var regex = new Regex(pattern.Value);
            var match = regex.Match(agencySegment);
            if (match.Success) {
                tagsDictionary[pattern.Key] = match.Groups["value"].Value.Trim();
            }
        }

        // Apply government patterns to the government segment
        foreach (var pattern in govPatterns) {
            var regex = new Regex(pattern.Value);
            var match = regex.Match(govSegment);
            if (match.Success) {
                tagsDictionary[pattern.Key] = match.Groups["value"].Value.Trim();
            }
        }

        int nextTitleStartIndex = inputText.IndexOf("Uprava za", phoneIndex + 1, StringComparison.Ordinal);
        string title = inputText
            .Substring(phoneIndex + 1, nextTitleStartIndex - phoneIndex - 1);
        tagsDictionary.Add("NASLOV", title);



        int contentStartIndex = inputText
            .LastIndexOf("tekstu agencija", StringComparison.Ordinal);

        if (contentStartIndex == -1) {
            contentStartIndex = 0;
        }
        var clientSignatureToUse = "Potpis ugovora za klijenta";
        const string clientSigV2 = "Potpisnik ugovora za klijenta";
        var clientSigLength = clientSignatureToUse.Length;
        var contentLastIndex = inputText
            .IndexOf(clientSignatureToUse, StringComparison.Ordinal);

        if (contentLastIndex == -1) {
            clientSignatureToUse = clientSigV2;
            contentLastIndex = inputText
                .IndexOf(clientSignatureToUse, StringComparison.Ordinal);
            clientSigLength = clientSignatureToUse.Length;
            if (contentLastIndex == -1) {
                return tagsDictionary;
            }
        }
        var content = inputText
            .Substring(contentStartIndex + 1, contentLastIndex - contentStartIndex - 1);

        tagsDictionary.Add("SADRZAJ", content);

        // Extract the signatures if they exist
        if (contentLastIndex == textLength) {
            return tagsDictionary;
        }

        var govSigStart = contentLastIndex + clientSigLength;
        var agSignatureToUse = "Potpis ugovora za agenciju";
        const string agSigV2 = "Potpisnik ugovora za agenciju";
        var agSigLength = agSignatureToUse.Length;
        var govSigEndIndex = inputText
            .IndexOf(agSignatureToUse, StringComparison.Ordinal);
        if (govSigEndIndex == -1) {
            agSignatureToUse = agSigV2;
            govSigEndIndex = inputText
                .IndexOf(agSignatureToUse, StringComparison.Ordinal);
            agSigLength = agSignatureToUse.Length;
            if (govSigEndIndex == -1) {
                return tagsDictionary;
            }
        }
        var govSig = inputText
            .Substring(govSigStart + 1, govSigEndIndex - govSigStart - 1);
        tagsDictionary.Add("VLADA_POTPIS", govSig);

        var agSigStart = govSigEndIndex + agSigLength;

        var agSign = inputText
            .Substring(agSigStart + 1, textLength - agSigStart - 1);

        tagsDictionary.Add("AGECIJA_POTPIS", agSign);

        return tagsDictionary;
    }

    private static Contract ExtractParsedValues(Dictionary<string, string> extractedValues)
    {
        var document = new Contract();

        extractedValues.TryGetValue("NAZIV_ULICE_AGENCIJE", out var agencyStreet);
        extractedValues.TryGetValue("BROJ_ZGRADE_AGENCIJE", out var agencyBuildingNumber);
        extractedValues.TryGetValue("GRAD_AGENCIJE", out var agencyCity);
        extractedValues.TryGetValue("EMAIL_AGENCIJE", out var agencyEmail);
        extractedValues.TryGetValue("TELEFON_AGENCIJE", out var agencyPhone);

        var agencyFullAddress = $"{agencyStreet} {agencyBuildingNumber} {agencyCity}";
        var agencyAddress = new ContractAddress(agencyStreet, agencyBuildingNumber, agencyCity, agencyFullAddress);
        document.AgencyAddress = agencyAddress;
        document.AgencyEmail = agencyEmail;
        document.AgencyPhoneNumber = agencyPhone;

        extractedValues.TryGetValue("NAZIV_UPRAVE", out var clientName);
        extractedValues.TryGetValue("NIVO_UPRAVE", out var clientLevel);
        extractedValues.TryGetValue("NAZIV_ULICE", out var clientStreet);
        extractedValues.TryGetValue("BROJ_ZGRADE", out var clientBuildingNumber);
        extractedValues.TryGetValue("GRAD", out var clientCity);
        extractedValues.TryGetValue("EMAIL", out var clientEmail);
        extractedValues.TryGetValue("TELEFON", out var clientPhone);

        var governmentFullAddress = $"{clientStreet} {clientBuildingNumber} {clientCity}";
        var governmentAddress = new ContractAddress(clientStreet, clientBuildingNumber, clientCity, governmentFullAddress);
        document.ClientAddress = governmentAddress;
        document.ClientName = clientName;
        document.ClientLevel = clientLevel;
        document.ClientEmail = clientEmail;
        document.ClientPhoneNumber = clientPhone;

        extractedValues.TryGetValue("SADRZAJ", out var content);
        document.Content = content is not null
            ? content.PreProcessWithoutSpecialChars() : "";

        extractedValues.TryGetValue("VLADA_POTPIS", out var govSign);
        extractedValues.TryGetValue("AGECIJA_POTPIS", out var agSign);
        var gov = govSign?.PreProcessWithoutSpecialChars();
        var govNameSurname = GetNameAndSurname(gov);
        var governmentSign = new SignatureExtracted(govNameSurname.Item1, govNameSurname.Item2, gov);
        document.ClientSign = governmentSign;

        var ag = agSign?.PreProcessWithoutSpecialChars();
        var agNameSurname = GetNameAndSurname(ag);
        var agencySign = new SignatureExtracted(agNameSurname.Item1, agNameSurname.Item2, ag);
        document.AgencySign = agencySign;

        return document;
    }
    private static (string?, string?) GetNameAndSurname(string? input)
    {
        var trimmedInput = input?.Trim();

        var nameParts = trimmedInput?.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (nameParts is not { Length: > 0 }) return (null, null);
        if (nameParts.Length == 1)
        {
            return (nameParts[0], string.Empty);
        }

        var firstName = nameParts[0];
        var surname = string.Join(" ", nameParts.Skip(1));
        return (firstName, surname);

    }
}