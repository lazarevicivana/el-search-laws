import {Component, EventEmitter, Input, Output} from '@angular/core';
import {FormControl, ReactiveFormsModule} from "@angular/forms";

@Component({
  selector: 'app-full-text-search',
  standalone: true,
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './full-text-search.component.html',
  styleUrl: './full-text-search.component.scss'
})
export class FullTextSearchComponent {
  @Input() searchValue : FormControl = new FormControl<string>('')
  @Output() performSearch : EventEmitter<any> = new EventEmitter<any>()

  search() {
    this.performSearch.emit()
  }
}
