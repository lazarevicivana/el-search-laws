import {Component, OnInit} from '@angular/core';
import {GoogleMapsModule} from "@angular/google-maps";
import {NgForOf, NgIf} from "@angular/common";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-geo-search',
  standalone: true,
  imports: [GoogleMapsModule, NgForOf, FormsModule, NgIf],
  templateUrl: './geo-search.component.html',
  styleUrl: './geo-search.component.scss'
})
export class GeoSearchComponent implements OnInit{
  center: google.maps.LatLngLiteral = {lat: 45.2671, lng: 19.8335}; // Coordinates for Novi
  zoom = 12;
  markers: google.maps.MarkerOptions[] = []; // This will hold all markers
  circle: google.maps.CircleOptions | null = null;
  options: google.maps.CircleOptions | null = null
  onMapClick(event: google.maps.MapMouseEvent) {
    // Clear previous markers if you only want one at a time
    this.markers = [];

    if (event.latLng) {
      const lat = event.latLng.lat();
      const lng = event.latLng.lng();
      this.circle = {
        center: { lat, lng },
        radius: 1000,
        strokeColor: '#blue',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.35,
        clickable: false,
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
      editable: true, // Allows the user to change the radius
      draggable: false,
    }
  }

  onRadiusChange($event: number) {
  }
}
