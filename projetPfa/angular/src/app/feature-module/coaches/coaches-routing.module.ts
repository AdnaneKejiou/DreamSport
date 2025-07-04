import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CoachesComponent } from './coaches.component';

const routes: Routes = [
  {
    path: '',
    component: CoachesComponent,
    children: [
      
    ],
  },
  {
    path: 'pages',
    loadChildren: () =>
      import('./pages/pages.module').then((m) => m.PagesModule),
  },
  
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CoachesRoutingModule {}
