import { Component } from '@angular/core';
import { routes } from 'src/app/core/helpers/routes';
import { bloglist, latestpost } from 'src/app/core/models/models';
import { DataService } from 'src/app/core/service/data/data.service';

@Component({
  selector: 'app-blog-list-sidebar-right',
  templateUrl: './blog-list-sidebar-right.component.html',
  styleUrls: ['./blog-list-sidebar-right.component.scss']
})
export class BlogListSidebarRightComponent {
  public routes = routes
  public latestpost: latestpost[] =[];
  public bloglist : bloglist[] =[];
  constructor (private dataservice:DataService){
    (this.latestpost = this.dataservice.latestpost),
    (this.bloglist = this.dataservice.bloglist)
  }
  fav(data: bloglist){
    data.favourite = !data.favourite
  }


}
