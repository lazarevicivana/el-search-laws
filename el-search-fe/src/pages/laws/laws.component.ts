import {Component, inject, OnInit} from '@angular/core';
import {FullTextSearchComponent} from "../../shared/full-text-search/full-text-search.component";
import {FormControl} from "@angular/forms";
import {LawService} from "../../core/services/law.service";
import {SearchRequest} from "../../core/request/search-request";
import {take} from "rxjs";
import {HttpClientModule} from "@angular/common/http";
import {Hit} from "../../core/responses/search-law-response";
import {NgForOf, NgIf} from "@angular/common";
import {SearchResultComponent} from "../../shared/search-result/search-result.component";

@Component({
    selector: 'app-laws',
    standalone: true,
    imports: [
        FullTextSearchComponent,
        NgIf,
        SearchResultComponent,
        NgForOf,
    ],
    providers: [LawService],
    templateUrl: './laws.component.html',
    styleUrl: './laws.component.scss'
})
export class LawsComponent implements OnInit {
    searchValue = new FormControl<string>('')
    foundResult : boolean = false
    private readonly service = inject(LawService)
    hits : Hit[]=[]

    ngOnInit(): void {
    }

    onPerformSearch() {
        this.foundResult = false
        this.hits = []
        const request: SearchRequest = {
            query: this.searchValue.value!
        }
        this.service.search(request)
            .pipe(take(1))
            .subscribe(x => {
               this.hits =[...x.hits]
                this.foundResult = true
            })
    }

}
