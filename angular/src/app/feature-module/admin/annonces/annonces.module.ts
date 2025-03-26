import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AnnoncesRoutingModule } from './annonces-routing.module';
import { AnnoncesComponent } from './annonces.component';


@NgModule({
  declarations: [
    AnnoncesComponent
  ],
  imports: [
    CommonModule,
    AnnoncesRoutingModule
  ]
})
export class AnnoncesModule { }
