import { DatePipe } from '@angular/common';
import { Component, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { routes } from 'src/app/core/core.index';
interface data {
  value: string;
}

@Component({
  selector: 'app-add-court',
  templateUrl: './add-court.component.html',
  styleUrl: './add-court.component.scss'
})
export class AddCourtComponent {
  public routes = routes
  public isChecked = false;
  public isChecked1 = false;
  public isChecked2 = false;
  public isChecked3 = false;
  public isChecked4 = false;
  public isChecked5 = false;
  showTimePicker: Array<string> = []
  startTime1 = new Date();
  startTime2 = new Date();
  startTime3 = new Date();
  startTime4 = new Date();
  startTime5 = new Date();
  startTime6 = new Date();
  startTime7 = new Date();
  endTime1 = new Date();
  endTime2 = new Date();
  endTime3 = new Date();
  endTime4 = new Date();
  endTime5 = new Date();
  endTime6 = new Date();
  endTime7 = new Date();

  public selectedValue1 = '';
  public selectedValue2 = '';
  public selectedValue3 = '';
  public selectedValue4 = '';
  public selectedValue5 = '';
  public selectedValue6 = '';
  public selectedValue7 = '';
  public selectedValue8 = '';
  public dltImg1  = true;
  public dltImg2  = true;
  public dltImg3  = true;
  public dltImg4  = true;
  


  selectedList1: data[] = [{ value: '1 Hrs' }, { value: '2 Hrs' }, { value: '3 Hrs' }];
  selectedList2: data[] = [{ value: '1 Hrs' }, { value: '2 Hrs' }, { value: '3 Hrs' }];
  selectedList3: data[] = [{ value: '1 Hrs' }, { value: '2 Hrs' }, { value: '3 Hrs' }];
  selectedList4: data[] = [{ value: '1 Hrs' }, { value: '2 Hrs' }, { value: '3 Hrs' }];
  selectedList5: data[] = [{ value: '1 Hrs' }, { value: '2 Hrs' }, { value: '3 Hrs' }];
  selectedList6: data[] = [{ value: '1 Hrs' }, { value: '2 Hrs' }, { value: '3 Hrs' }];
  selectedList7: data[] = [{ value: '1 Hrs' }, { value: '2 Hrs' }, { value: '3 Hrs' }];
  selectedList8: data[] = [{ value: 'Select Court Type' }, { value: 'Tornoto' }, { value: 'Texas' }];

  adult = 1;
  children = 1;
  young_children = 1;
  constructor( private datePipe: DatePipe,private renderer: Renderer2) { 

  }
  
  formatTime(date: Date) {
    const selectedDate: Date = new Date(date)
    return this.datePipe.transform(selectedDate, 'h:mm a')
  }
  toggleTimePcker(value: string): void {

    if (this.showTimePicker[0] !== value) {
      this.showTimePicker[0] = value
    } else {
      this.showTimePicker = []
    }
  }
  images = [
    { isVisible: true },
    { isVisible: true },
    { isVisible: true }
  ];

  dltImage4(){
    this.dltImg4 = !this.dltImg4
  }
  toggleImage(index: number): void {
    this.images[index].isVisible = !this.images[index].isVisible;
  }
 
  

  scrollToSection(section: HTMLElement) {
    if (section) {
      this.scrollTo(section);
    }
  }

  scrollTo(element: any) {
    element.scrollIntoView({ behavior: 'smooth', block: 'start' });
  }
  activeTab: string = 'basicInfo';
  
  
  setActiveTab(tabName: string) {
    this.activeTab = tabName;
  }
}
