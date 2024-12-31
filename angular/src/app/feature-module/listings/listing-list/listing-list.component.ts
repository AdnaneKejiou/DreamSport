import { Component } from '@angular/core';
import { routes } from 'src/app/core/helpers/routes';
import { sports } from 'src/app/core/models/models';
import { DataService } from 'src/app/core/service/data/data.service';
interface data {
  value: string;
}

@Component({
  selector: 'app-listing-list',
  templateUrl: './listing-list.component.html',
  styleUrls: ['./listing-list.component.scss']
})
export class ListingListComponent {
  isClassAdded: boolean[] = [false, false, false];
  toggleClass(index: number) {
    this.isClassAdded[index] = !this.isClassAdded[index];
  }
  fav(slide:sports){
    slide.favourite = !slide.favourite
  }
  public routes = routes;
  public sports: sports[] = [];
  constructor (private dataservice:DataService){
    (this.sports = this.dataservice.sports)
  }
  public selectedValue1 = '';

  selectedList1: data[] = [
    {value: 'Relevance'},
    {value: 'Price'}
  ];

}
