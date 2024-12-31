import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoachesGridRoutingModule } from './coaches-grid-routing.module';
import { CoachesGridComponent } from './coaches-grid.component';
import { sharedModule } from 'src/app/shared/shared.module';


@NgModule({
  declarations: [
    CoachesGridComponent
  ],
  imports: [
    CommonModule,
    CoachesGridRoutingModule,
    sharedModule
  ]
})
export class CoachesGridModule { }
