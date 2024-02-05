import {Routes} from '@angular/router';
import {LawsComponent} from "../pages/laws/laws.component";

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
    component: LawsComponent
  },
];
