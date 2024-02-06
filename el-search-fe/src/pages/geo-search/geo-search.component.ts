import {Component, inject, OnInit} from '@angular/core';
import {GoogleMapsModule} from "@angular/google-maps";
import {NgForOf, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {ContractService} from "../../core/services/contract.service";
import {GeoSearch} from "../../core/request/geo-search";
import {ContractHit} from "../../core/responses/search-contract-response";
import {SearchResultComponent} from "../../shared/search-result/search-result.component";
import {downloadFile} from "../../shared/download-file";
import {LawService} from "../../core/services/law.service";

@Component({
  selector: 'app-geo-search',
  standalone: true,
  imports: [GoogleMapsModule, NgForOf, FormsModule, NgIf, SearchResultComponent],
  providers:[ContractService,LawService],
  templateUrl: './geo-search.component.html',
  styleUrl: './geo-search.component.scss'
})
export class GeoSearchComponent implements OnInit{
  center: google.maps.LatLngLiteral = {lat: 45.2671, lng: 19.8335}; // Coordinates for Novi
  zoom = 12;
  markers: google.maps.MarkerOptions[] = []; // This will hold all markers
  circle: google.maps.CircleOptions | null = null;
  options: google.maps.CircleOptions | null = null
  latitude: number = 45.2671
  longitude:number = 19.8335
  hits : ContractHit[]=[]
  private readonly service = inject(ContractService)
  private readonly lawService = inject(LawService)
  onMapClick(event: google.maps.MapMouseEvent) {
    // Clear previous markers if you only want one at a time
    this.markers = [];

    if (event.latLng) {
      const lat = event.latLng.lat();
      const lng = event.latLng.lng();
      this.latitude = lat
      this.longitude = lng
      this.circle = {
        center: { lat, lng },
        radius: 1000,
        strokeColor: '#blue',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.35,
        clickable: true,
        editable: true, // Allows the user to change the radius
        draggable: false,
      };
      // Add marker to the markers array
      this.markers.push({
        position: {
          lat: lat,
          lng: lng,
        },
        // You can add any additional options here
      });

      // Log the latitude and longitude or handle them as needed
      console.log(`Latitude: ${lat}, Longitude: ${lng}`);
    }
  }
  updateCircleRadius() {
    // The circle will automatically update since its radius property is bound to the input's value
    console.log(`Updated circle radius: ${this.circle?.radius} meters`);
  }

  ngOnInit(): void {
    this.options = {
      strokeColor: '#78A1BB',
      strokeOpacity: 0.8,
      strokeWeight: 2,
      fillColor: '#78A1BB',
      fillOpacity: 0.35,
      clickable: false,
      editable: false, // Allows the user to change the radius
      draggable: false,
    }
  }

  onRadiusChange($event: number) {
  }

  onSearch() {
    console.log(this.circle?.radius)
    this.hits = []
    const request : GeoSearch={
      lat: this.latitude,
      long: this.longitude,
      distance: this.circle?.radius ? this.circle.radius : 0
    }
    this.service.searchGeo(request)
        .subscribe(x => {
          this.hits = [... x.hits]
        })
  }
  onDownload($event: string) {
    this.lawService.downloadDocument($event,1)
        .subscribe(x => {
          downloadFile("contract.pdf",x)
        })
  }
}
