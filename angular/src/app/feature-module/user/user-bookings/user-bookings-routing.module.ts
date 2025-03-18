import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserBookingsComponent } from './user-bookings.component';

const routes: Routes = [{ path: '', component: UserBookingsComponent,
children: [
  {
    path: 'details',
    loadChildren: () =>
      import('./details/details.module').then(
        (m) => (m).DetailsModule
      ),
      
  },
  {
    path: 'timedate/:id',
    loadChildren: () =>
      import('./timedate/timedate.module').then(
        (m) => (m).CoachTimedateModule
      ),
  },
  { path: 'timedate',
     redirectTo: 'details',
      pathMatch: 'full' }


]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserBookingsRoutingModule { }
