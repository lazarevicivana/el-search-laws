import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {httpOptions, httpOptionsFile} from "./http.options";
import {SearchRequest} from "../request/search-request";
import {SearchLawResponse} from "../responses/search-law-response";
import {UploadFile} from "../request/upload-file";

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
  public downloadDocument(fileName: string, type: number):Observable<Blob>{
    return this.http.get<Blob>(` https://localhost:7048/api/v1/contract/${fileName}/${type}`,{responseType: 'blob' as 'json'})
  }

    uploadLaw(request: FormData) {
      return this.http.post<SearchLawResponse>(`https://localhost:7048/api/v1/law`, request, httpOptionsFile)
    }
}
