import { Component } from '@angular/core';
import { routes } from 'src/app/core/helpers/routes';
import { DataService } from 'src/app/core/service/data/data.service';
import { events } from 'src/app/core/models/models';
@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent {
  public routes = routes
  public events: events[] = [];
  constructor (private dataservice:DataService){
    this.events= this.dataservice.events
  
}
}

