import { Component } from '@angular/core';
import { routes } from 'src/app/core/helpers/routes';
import { CommonService } from 'src/app/core/service/common/common.service';

@Component({
  selector: 'app-cage',
  templateUrl: './cage.component.html',
  styleUrl: './cage.component.scss'
})
export class CageComponent {
  public routes = routes;
base = '';
page = '';
last = '';
constructor(
  private common: CommonService,
){
  this.common.base.subscribe((res: string) => {
    this.base = res;
  });
  this.common.page.subscribe((res: string) => {
    this.page = res;
  });
  this.common.last.subscribe((res: string) => {
    this.last = res;
  });
}

}
