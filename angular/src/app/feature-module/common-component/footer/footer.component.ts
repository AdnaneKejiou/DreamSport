import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/core.index';
import { selectTenantData } from 'src/app/store/tenant/tenant.selectors';
interface data {
  value: string;
}
@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  public routes = routes;
  public selectedValue1 = '';
  public selectedValue2 = '';
  tenantData$: Observable<any>;
  
   constructor(private store: Store) {
        this.tenantData$ = this.store.select(selectTenantData);
         
   }

  selectedList1: data[] = [{ value: 'English (US)' }, { value: 'UK' }, { value: 'Japan' }];
  selectedList2: data[] = [{ value: '$ USD' }, { value: '$ Euro' }];
}
