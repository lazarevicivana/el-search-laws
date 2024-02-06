export function downloadFile(fileName: string, blobFile: Blob) {
    const blob = new Blob([blobFile]);
    const downloadUrl = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = downloadUrl;
    link.download = fileName;
    link.click();
}
export function highLight(htmlString: string) {
    // Define the replacement tag with the desired styles
    const replacementStartTag = '<span style="background-color: red;">';
    const replacementEndTag = '</span>';
    console.log(htmlString)
    // Replace <em> start tags with the replacement start tag
    return htmlString.replace(/<em>/g, replacementStartTag)
        .replace(/<\/em>/g, replacementEndTag);
}
