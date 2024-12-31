import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChangePasswordRoutingModule } from './change-password-routing.module';
import { ChangePasswordComponent } from './change-password.component';
import { sharedModule } from 'src/app/shared/shared.module';
import { FeatherIconModule } from 'src/app/shared/model/feather.module';


@NgModule({
  declarations: [
    ChangePasswordComponent
  ],
  imports: [
    CommonModule,
    ChangePasswordRoutingModule,
    sharedModule,
    FeatherIconModule
  ]
})
export class ChangePasswordModule { }
