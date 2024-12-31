import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompletedRoutingModule } from './completed-routing.module';
import { CompletedComponent } from './completed.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CompletedComponent
  ],
  imports: [
    CommonModule,
    CompletedRoutingModule,
    sharedModule
  ]
})
export class CompletedModule { }
