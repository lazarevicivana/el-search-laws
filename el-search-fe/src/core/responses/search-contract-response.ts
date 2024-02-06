export interface SearchContractResponse{
 hits: ContractHit[]
}
export interface ContractHit{
    governmentName:string,
    governmentType: string,
    signatoryPersonSurname:string,
    signatoryPersonName:string,
    highlight:string,
    content:string,
    contractId:string,
    fileName :string
}
