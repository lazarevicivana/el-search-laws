import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {httpOptions} from "./http.options";
import {SearchRequest} from "../request/search-request";
import {SearchLawResponse} from "../responses/search-law-response";

@Injectable({
  providedIn: 'root'
})
export class LawService {
  private readonly http = inject(HttpClient)
  private readonly baseUrl = 'https://localhost:7251/api/v1/law'

  constructor() {
  }

  search(request: SearchRequest): Observable<SearchLawResponse> {
    return this.http.post<SearchLawResponse>(`${this.baseUrl}`, request, httpOptions)
  }

}
