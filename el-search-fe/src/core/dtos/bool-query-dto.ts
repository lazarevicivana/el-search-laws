export interface BoolQueryDto{
    operator : string,
    value: string,
    field: string,
    isPhrase: boolean,
    counter: number
}
