import { Component } from '@angular/core';
import { routes } from 'src/app/core/core.index';

@Component({
  selector: 'app-othersetting',
  templateUrl: './othersetting.component.html',
  styleUrls: ['./othersetting.component.scss']
})
export class OthersettingComponent {
  public routes = routes;

}
