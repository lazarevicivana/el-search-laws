import {Routes} from '@angular/router';
import {LawsComponent} from "../pages/laws/laws.component";
import {ContractsComponent} from "../pages/contracts/contracts.component";
import {GeoSearchComponent} from "../pages/geo-search/geo-search.component";

export const routes: Routes = [
  { path: '',
    redirectTo: 'laws',
    pathMatch: 'full'
  },
  {
    path: 'laws',
    component: LawsComponent
  },
  {
    path: 'contracts',
    component: ContractsComponent
  },
  {
    path: 'geo-search',
    component: GeoSearchComponent
  },
];
