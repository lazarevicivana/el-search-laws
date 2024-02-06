import {Component, inject, OnInit} from '@angular/core';
import {FullTextSearchComponent} from "../../shared/full-text-search/full-text-search.component";
import {Form, FormControl, ReactiveFormsModule} from "@angular/forms";
import {LawService} from "../../core/services/law.service";
import {SearchRequest} from "../../core/request/search-request";
import {take} from "rxjs";
import {HttpClientModule} from "@angular/common/http";
import {Hit} from "../../core/responses/search-law-response";
import {NgForOf, NgIf} from "@angular/common";
import {SearchResultComponent} from "../../shared/search-result/search-result.component";
import {downloadFile, highLight} from "../../shared/download-file";
import {log} from "@angular-devkit/build-angular/src/builders/ssr-dev-server";
import {UploadFile} from "../../core/request/upload-file";
import {ToastrService} from "ngx-toastr";

@Component({
    selector: 'app-laws',
    standalone: true,
    imports: [
        FullTextSearchComponent,
        NgIf,
        SearchResultComponent,
        NgForOf,
        ReactiveFormsModule,
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
    selectedFile: any;
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

    onDownload($event: string) {
        this.service.downloadDocument($event,0)
            .subscribe(x => {
                downloadFile("law.pdf",x)
            })
    }

    protected readonly highLight = highLight;

    uploadLaw() {
        if (this.selectedFile) {
            const formData = new FormData();
            formData.append('file', this.selectedFile);
        const request : UploadFile={
            file: this.selectedFile
        }
        this.service.uploadLaw(this.selectedFile)
            .subscribe(x => {
                alert("Document is uploaded")
            })
    }
    }
    onFileChanged($event: any) {
        const file: File = $event.target.files[0];
        if (file instanceof File) {
            const reader = new FileReader();
            reader.onload = (e) => {
              this.selectedFile = e.target?.result
            };
            reader.readAsDataURL(file);

        } else {
            console.error("Selected item is not a File:", file);
        }
    }
}
