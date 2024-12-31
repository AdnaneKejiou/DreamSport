import { Component } from '@angular/core';
import { routes } from 'src/app/core/helpers/routes';
import { badmintonList } from 'src/app/core/models/models';

import { DataService } from 'src/app/core/service/data/data.service';

@Component({
  selector: 'app-blog-grid',
  templateUrl: './blog-grid.component.html',
  styleUrls: ['./blog-grid.component.scss']
})
export class BlogGridComponent {
  public routes = routes
  public badmintonList: badmintonList[] = [];
  constructor (private dataservice:DataService){
    (this.badmintonList = this.dataservice.badmintonList)
  }
  fav(slide: badmintonList){
    slide.favourite = !slide.favourite
  }

}
