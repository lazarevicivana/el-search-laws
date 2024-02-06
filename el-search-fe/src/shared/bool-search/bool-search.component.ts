import {Component, Input, OnInit} from '@angular/core';
import {FormControl, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {NgForOf, NgIf} from "@angular/common";

@Component({
  selector: 'app-bool-search',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    NgForOf,
    NgIf
  ],
  templateUrl: './bool-search.component.html',
  styleUrl: './bool-search.component.scss'
})
export class BoolSearchComponent implements OnInit{
 @Input() bool  = new FormGroup({
    value: new FormControl(''),
    isPhrase: new FormControl(false),
    field: new FormControl("Government"),
    counter : new FormGroup<number>(0),
    operator: new FormGroup('')
});
  options= [
    { value: 'governmentName', label: 'Government' },
    { value: 'governmentType', label: 'Government Level' },
    { value: 'signatoryPersonName', label: 'Name' },
    { value: 'signatoryPersonSurname', label: 'Surname' },
    { value: 'content', label: 'Content' },

    // Add more options as needed
  ];
  operators= [
    { value: ' AND ', label: 'AND' },
    { value: ' OR ', label: 'OR' },
    { value: ' AND NOT ', label: 'NOT' },
    // Add more options as needed
  ];


  addInput() {

  }

  performSearch() {

  }

  ngOnInit(): void {
  }
}
