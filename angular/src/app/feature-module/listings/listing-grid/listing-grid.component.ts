import { Component } from '@angular/core';
import { DataService, listinggrid, routes } from 'src/app/core/core.index';
interface data {
  value: string;
}

@Component({
  selector: 'app-listing-grid',
  templateUrl: './listing-grid.component.html',
  styleUrls: ['./listing-grid.component.scss']
})
export class ListingGridComponent {
  isClassAdded: boolean[] = [false, false, false];
  public listinggrid: listinggrid[] = [];
  constructor (private dataservice:DataService){
    (this.listinggrid = this.dataservice.listinggrid)
  }
  toggleClass(index: number) {
    this.isClassAdded[index] = !this.isClassAdded[index];
  }
  fav(slide:listinggrid){
    slide.favourite = !slide.favourite
  }
  public routes = routes;
  public selectedValue1 = '';
  selectedList1: data[] = [
    {value: 'Relevance'},
    {value: 'Price'}
  ];

}
