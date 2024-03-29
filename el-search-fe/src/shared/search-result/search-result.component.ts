import {Component, EventEmitter, inject, Input, OnInit, Output} from '@angular/core';
import {DomSanitizer, SafeHtml} from "@angular/platform-browser";

@Component({
    selector: 'app-search-result',
    standalone: true,
    imports: [],
    templateUrl: './search-result.component.html',
    styleUrl: './search-result.component.scss'
})
export class SearchResultComponent implements OnInit {
    @Input() text: string = ''
    @Input() fileName : string =''
    private readonly sanitizer = inject(DomSanitizer)
    @Output() download = new EventEmitter<string>()

    ngOnInit(): void {
    }

    onDownload() {
        this.download.emit(this.fileName)
    }
}
