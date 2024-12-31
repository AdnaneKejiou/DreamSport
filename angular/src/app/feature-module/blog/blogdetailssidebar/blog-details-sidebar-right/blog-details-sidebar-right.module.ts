import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BlogDetailsSidebarRightRoutingModule } from './blog-details-sidebar-right-routing.module';
import { BlogDetailsSidebarRightComponent } from './blog-details-sidebar-right.component';
import { sharedModule } from 'src/app/shared/shared.module';
import { FeatherIconModule } from 'src/app/shared/model/feather.module';


@NgModule({
  declarations: [
    BlogDetailsSidebarRightComponent
  ],
  imports: [
    CommonModule,
    BlogDetailsSidebarRightRoutingModule,
    sharedModule,
    FeatherIconModule
  ]
})
export class BlogDetailsSidebarRightModule { }
