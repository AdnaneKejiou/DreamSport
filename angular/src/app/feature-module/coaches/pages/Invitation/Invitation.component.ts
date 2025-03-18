import { Component } from '@angular/core';
import { routes } from 'src/app/core/core.index';

@Component({
  selector: 'app-Invitation',
  templateUrl: './Invitation.component.html',
  styleUrls: ['./Invitation.component.scss']
})
export class InvitationComponent {
  public routes = routes;
}
