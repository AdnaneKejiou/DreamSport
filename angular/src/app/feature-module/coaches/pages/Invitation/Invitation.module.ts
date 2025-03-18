import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InvitationRoutingModule } from './Invitation-routing.module';
import { InvitationComponent } from './Invitation.component';


@NgModule({
  declarations: [
    InvitationComponent
  ],
  imports: [
    CommonModule,
    InvitationRoutingModule
  ]
})
export class InvitationModule { }
