import {Component, inject, Input, OnInit} from '@angular/core';
import {DomSanitizer, SafeHtml} from "@angular/platform-browser";

@Component({
  selector: 'app-search-result',
  standalone: true,
  imports: [],
  templateUrl: './search-result.component.html',
  styleUrl: './search-result.component.scss'
})
export class SearchResultComponent implements OnInit{
@Input() text : string = ''
  safeHtml: SafeHtml | null = null;
private readonly sanitizer = inject(DomSanitizer)
  ngOnInit(): void {
    this.safeHtml = this.sanitizer.bypassSecurityTrustHtml(this.text)
  }
}
