import {Component, inject, OnInit} from '@angular/core';
import {FullTextSearchComponent} from "../../shared/full-text-search/full-text-search.component";
import {NgForOf, NgIf} from "@angular/common";
import {SearchResultComponent} from "../../shared/search-result/search-result.component";
import {FormArray, FormControl, FormGroup} from "@angular/forms";
import {LawService} from "../../core/services/law.service";
import {Hit} from "../../core/responses/search-law-response";
import {SearchRequest} from "../../core/request/search-request";
import {take} from "rxjs";
import {BoolSearchComponent} from "../../shared/bool-search/bool-search.component";
import {BoolQueryDto} from "../../core/dtos/bool-query-dto";
import {ContractService} from "../../core/services/contract.service";
import {ContractHit} from "../../core/responses/search-contract-response";
import {LawsComponent} from "../laws/laws.component";
import {downloadFile} from "../../shared/download-file";

@Component({
  selector: 'app-contracts',
  standalone: true,
    imports: [
        FullTextSearchComponent,
        NgForOf,
        NgIf,
        SearchResultComponent,
        BoolSearchComponent
    ],
    providers:[
        ContractService,
        LawService
    ],
  templateUrl: './contracts.component.html',
  styleUrl: './contracts.component.scss'
})
export class ContractsComponent implements OnInit{
    searchValue = new FormControl<string>('')
    foundResult : boolean = false
    bools = new FormArray<FormGroup>([])
    boolQueries : BoolQueryDto[]=[]
    private readonly service = inject(ContractService)
    private readonly lawService = inject(LawService)
    hits : ContractHit[]=[]

    ngOnInit(): void {
        this.bools.controls.push(this.createFormGroup(0,''))
    }
    private readonly createFormGroup = (counter: number,operator: string) =>
        new FormGroup({
            value: new FormControl(''),
            isPhrase: new FormControl(false),
            field: new FormControl("Government"),
            counter : new FormControl(counter),
            operator: new FormControl(operator)
        });

    addInput() {
        this.bools.push(this.createFormGroup(this.bools.length,' AND '));
    }

    performSearch() {
        this.hits = []
       const boolQueries = this.bools.controls.map(x =>{
            const bool: BoolQueryDto={
            counter: x.get('counter')?.value,
            field:  x.get('field')?.value,
            isPhrase:  x.get('isPhrase')?.value,
            operator:  x.get('operator')?.value,
            value:  x.get('value')?.value,
            }
            return bool
        })
        const query = this.buildQuery(boolQueries.sort(x => x.counter))
        console.log(query)
        const request : SearchRequest={
           query: query
        }
        this.service.search(request)
            .pipe(take(1))
            .subscribe(x => {
                this.hits = [... x.hits]
            })
    }

    private buildQuery(boolQueries: BoolQueryDto[]) {
        let query = ''
        let  notBoolQueries : BoolQueryDto[]=[]
        boolQueries.forEach(x =>{
            if(x.operator === ' AND NOT '){
               notBoolQueries = [
                   ...notBoolQueries,
                   x]
            }
            if(x.operator !== ' AND NOT '){
                const value = !x.isPhrase ? x.value : '\"'+x.value+'\"'
                query = query + x.operator + x.field + ':'+value
            }
        })
        console.log(query)
        if(notBoolQueries.length > 0){
            notBoolQueries.forEach(x => {
                const value = !x.isPhrase ? x.value : '\"'+x.value+'\"'
                query = query + x.operator + x.field + ':'+value
            })

        }
        return query
    }

    onDownload($event: string) {
        this.lawService.downloadDocument($event,1)
            .subscribe(x => {
                downloadFile("contract.pdf",x)
            })
    }
}
