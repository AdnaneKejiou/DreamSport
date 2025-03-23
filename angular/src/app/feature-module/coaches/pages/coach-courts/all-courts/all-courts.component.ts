import { Component } from '@angular/core';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DataService, apiResultFormat, pageSelection, routes } from 'src/app/core/core.index';
import { allcourts } from 'src/app/shared/model/page.model';
import { PaginationService, tablePageSize } from 'src/app/shared/shared.index';


interface data {
  value: string;
}
@Component({
  selector: 'app-all-courts',
  templateUrl: './all-courts.component.html',
  styleUrls: ['./all-courts.component.scss']
})
export class AllCourtsComponent {
  checked!: boolean;
  public selectedValue1 = '';
  public selectedValue2 = '';
  selectedTab: string = 'all';
  initChecked = false;
  public tableData: Array<allcourts> = [];
  public tableShowed: Array<allcourts> = [];
  public routes = routes;
  // pagination variables
  public pageSize = 10;
  public serialNumberArray: Array<number> = [];
  public totalData = 0;
  showFilter = false;
  dataSource!: MatTableDataSource<allcourts>;
  public searchDataValue = '';
  selectedList1: data[] = [
    {value: 'Relevance'},
    {value: 'Price'},
  ];
  selectedList2: data[] = [
    {value: 'This Week'},
    {value: 'One Day'},
  ];
  constructor(
    private data: DataService,
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

  private getTableData(pageOption: pageSelection): void {
    this.data.getAllCourts().subscribe((apiRes: apiResultFormat) => {
      this.tableData = [];
      this.serialNumberArray = [];
      this.totalData = apiRes.totalData;
      apiRes.data.map((res: allcourts, index: number) => {
        const serialNumber = index + 1;
        if (index >= pageOption.skip && serialNumber <= pageOption.limit) {
          res.id = serialNumber;
          this.tableData.push(res);
          this.tableShowed=this.tableData;
          this.serialNumberArray.push(serialNumber);
        }
      });
      this.dataSource = new MatTableDataSource<allcourts>(this.tableData);
      this.pagination.calculatePageSize.next({
        totalData: this.totalData,
        pageSize: this.pageSize,
        tableData: this.tableData,
        serialNumberArray: this.serialNumberArray,
      });
    });
  }

  public searchData(value: string): void {
    this.dataSource.filter = value.trim().toLowerCase();
    this.tableData = this.dataSource.filteredData;
  }

  public sortData(sort: Sort) {
    const data = this.tableData.slice();

    if (!sort.active || sort.direction === '') {
      this.tableData = data;
    } else {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      this.tableData = data.sort((a: any, b: any) => {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const aValue = (a as any)[sort.active];
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const bValue = (b as any)[sort.active];
        return (aValue < bValue ? -1 : 1) * (sort.direction === 'asc' ? 1 : -1);
      });
    }
  }

  public activateCourts() {
    this.tableShowed = this.tableData.filter(item => item.status === true);
    this.selectedTab = 'active';
  }

  public inactiveCourts() {
    this.tableShowed = this.tableData.filter(item => item.status === false);
    this.selectedTab = 'inactive';
  } 
  public allCourts(){
    this.tableShowed = this.tableData;
    this.selectedTab = 'all';
  }
}
