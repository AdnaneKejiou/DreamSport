import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OwlOptions } from 'ngx-owl-carousel-o';
import { routes } from 'src/app/core/core.index'; // Importez routes
import { TerrainService } from 'src/app/core/service/terrain/terrain.service';

@Component({
  selector: 'app-timedate',
  templateUrl: './timedate.component.html',
  styleUrls: ['./timedate.component.scss']
})
export class TimedateComponent implements OnInit {
  public routes = routes; 

  public dates: { date: string, day: string }[] = [];
  public selectedDate: { date: string, day: string } | null = null;
  public selectedTime: string | null = null;
  public selectedTimes: string[] = [];
  public endTime: string = '';
  public totalHours: number = 0;
  public subtotal: number = 0;
  public terrainId: number | null = null;


  // Static list of available times for demonstration
  private availableTimes: string[] = ['2:00pm', '3:00pm', '4:00pm', '5:00pm', '6:00pm', '7:00pm'];

  constructor(
    private route: ActivatedRoute,
    private terrainService: TerrainService
  ) {}

  ngOnInit(): void {
    
    this.generateDates();

    this.route.paramMap.subscribe((params) => {
      const id = params.get('id');
      if (id) {
        this.terrainId = +id;
      }
      
    });
  }

  public coachTimeDateOptions: OwlOptions = {
    loop: true,
    margin: 24,
    nav: true,
    dots: false,
    autoplay: false,
    smartSpeed: 2000,
    navText: ["<i class='feather icon-chevron-left'></i>", "<i class='feather icon-chevron-right'></i>"],
    responsive: {
      0: {
        items: 1
      },
      500: {
        items: 4
      },
      768: {
        items: 3
      },
      1000: {
        items: 4
      }
    }
  }

  generateDates(): void {
    const today = new Date();
    for (let i = 0; i < 7; i++) {
      const date = new Date(today);
      date.setDate(today.getDate() + i);
      this.dates.push({
        date: date.toLocaleDateString('en-US', { day: 'numeric', month: 'short' }),
        day: date.toLocaleDateString('en-US', { weekday: 'long' })
      });
    }
  }

  selectDate(date: { date: string, day: string }): void {
    this.selectedDate = date;
    this.selectedTimes = this.availableTimes; // Assign available times to selectedTimes
    this.selectedTime = null;
    this.endTime = '';
    this.totalHours = 0;
    this.subtotal = 0;
  }

  selectTime(time: string): void {
    this.selectedTime = time;
    const timeIndex = this.selectedTimes.indexOf(time);
    this.endTime = this.selectedTimes[timeIndex + 1] || '';
    this.totalHours = 1; // Assuming each slot is 1 hour
    this.subtotal = this.totalHours * 100; // Assuming $100 per hour
  }
}