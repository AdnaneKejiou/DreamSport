import { Component } from '@angular/core';
import { routes } from 'src/app/core/core.index';
interface data {
  value: string;
}
@Component({
  selector: 'app-coach-profile',
  templateUrl: './coach-profile.component.html',
  styleUrls: ['./coach-profile.component.scss'],
})
export class CoachProfileComponent {
  public routes = routes;


}
