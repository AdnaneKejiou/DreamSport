import { Component } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { PaginationService, tablePageSize } from 'src/app/shared/shared.index';
import { Router } from '@angular/router';
import {  pageSelection, routes } from 'src/app/core/core.index';
import { User } from 'src/app/core/models/user.model'

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {
  public searchDataValue = '';
  public tableShowed : Array<User> = []; //list that holds the list of users
  public tableData: Array<User> = [
    {
      id: 1,
      firstName: 'John',
      lastName: 'Doe',
      email: 'john.doe@example.com',
      imageUrl: 'https://example.com/images/john.jpg',
      username: 'johndoe',
      isBlocked: false,
      phoneNumber: '123-456-7890'
    },
    {
      id: 2,
      firstName: 'Jane',
      lastName: 'Smith',
      email: 'jane.smith@example.com',
      imageUrl: 'https://images.unsplash.com/photo-1575936123452-b67c3203c357?fm=jpg&q=60&w=3000&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8Mnx8aW1hZ2V8ZW58MHx8MHx8fDA%3D',
      username: 'janesmith',
      isBlocked: true,
      phoneNumber: '987-654-3210'
    },
    {
      id: 3,
      firstName: 'Alice',
      lastName: 'Johnson',
      email: 'alice.johnson@example.com',
      imageUrl: 'https://example.com/images/alice.jpg',
      username: 'alicej',
      isBlocked: false,
      phoneNumber: '555-123-4567'
    },
    {
      id: 4,
      firstName: 'Bob',
      lastName: 'Brown',
      email: 'bob.brown@example.com',
      imageUrl: 'https://example.com/images/bob.jpg',
      username: 'bobbrown',
      isBlocked: true,
      phoneNumber: '444-555-6666'
    },
    {
      id: 5,
      firstName: 'Charlie',
      lastName: 'Davis',
      email: 'charlie.davis@example.com',
      imageUrl: 'https://example.com/images/charlie.jpg',
      username: 'charlied',
      isBlocked: false,
      phoneNumber: '777-888-9999'
    }
  ]; //list that holds users tha will be showed
  public routes = routes;
  public pageSize = 10;
  selectedTab: string = 'all';

  constructor(
      private router: Router,
      private pagination: PaginationService
    ){
   
      this.pagination.tablePageSize.subscribe((res: tablePageSize) => {
        if (this.router.url === this.routes.coachCourts) {
          this.getTableData({ skip: res.skip, limit: res.limit });
          this.pageSize = res.pageSize;
        }
      });
      
    }


  public searchData(value: string): void {
    //search methode 
  }

  //sort methode dyal dok les fleches
  public sortData(sort: Sort) {
      const data = this.tableShowed.slice();
  
      if (!sort.active || sort.direction === '') {
        this.tableShowed = data;
      } else {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        this.tableShowed = data.sort((a: any, b: any) => {
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          const aValue = (a as any)[sort.active];
          // eslint-disable-next-line @typescript-eslint/no-explicit-any
          const bValue = (b as any)[sort.active];
          return (aValue < bValue ? -1 : 1) * (sort.direction === 'asc' ? 1 : -1);
        });
      }
  }


  private getTableData(pageOption: pageSelection): void {
      //get data from server
  }


  public blocked() {
    this.tableShowed = this.tableData.filter(item => item.isBlocked === true);//need adjust
    this.selectedTab = 'blocked';
  }

  public inblocked() {
    this.tableShowed = this.tableData.filter(item => item.isBlocked === false);//need adjust
    this.selectedTab = 'notBlocked';
  } 
  public allUsers(){
    this.tableShowed = this.tableData;//need adjust
    this.selectedTab = 'all';
  }
}
