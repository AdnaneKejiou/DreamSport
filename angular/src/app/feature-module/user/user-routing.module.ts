import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserComponent } from './user.component';

const routes: Routes = [
  {
    path: '',
    component: UserComponent,
    data: { breadcrumbLabel: 'Home' },
    children: [
      {
        path: 'user-dashboard',
        loadChildren: () =>
          import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
      },

      {
        path: 'chat',
        loadChildren: () =>
          import('./chat/chat.module').then((m) => m.ChatModule),
      },
      {
        path: 'invitation',
        loadChildren: () =>
          import('./Invitation/Invitation.module').then((m) => m.InvitationModule),
      },
      {
        path: 'team-invitation',
        loadChildren: () =>
          import('./team/team.module').then((m) => m.TeamModule),
      },
      
      {
        path: 'user-profile',
        loadChildren: () =>
          import('./profile/profile.module').then((m) => m.ProfileModule),
      },
      {
        path: 'change-password',
        loadChildren: () =>
          import('./change-password/change-password.module').then(
            (m) => m.ChangePasswordModule
          ),
      },
      {
        path: 'user-profile-setting',
        loadChildren: () =>
          import('./other-settings/other-settings.module').then(
            (m) => m.OtherSettingsModule
          ),
      },
      {
        path: 'user-bookings',
        loadChildren: () =>
          import('./user-bookings/user-bookings.module').then(
            (m) => m.UserBookingsModule
          ),
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class UserRoutingModule {}
