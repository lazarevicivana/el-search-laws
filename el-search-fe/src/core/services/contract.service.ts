import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {SearchRequest} from "../request/search-request";
import {Observable} from "rxjs";
import {SearchLawResponse} from "../responses/search-law-response";
import {httpOptions} from "./http.options";
import {SearchContractResponse} from "../responses/search-contract-response";

@Injectable({
  providedIn: 'root'
})
export class ContractService {
  private readonly http = inject(HttpClient)
  private readonly baseUrl = 'https://localhost:7251/api/v1/contract'

  constructor() {
  }

  search(request: SearchRequest): Observable<SearchContractResponse> {
    return this.http.post<SearchContractResponse>(`${this.baseUrl}/bool`, request, httpOptions)
  }
}
