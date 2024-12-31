import { Component, NgZone, OnInit } from '@angular/core';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { routes } from 'src/app/core/helpers/routes';

interface data {
  value: string;
}
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  public selectedValue1 = '';
  selectedList1: data[] = [
    {value: 'Country'},
    {value: 'Texas'},
  ];
  public routes = routes;
  public profileSettingsForm!: UntypedFormGroup;
  constructor(
    private formBuilder: UntypedFormBuilder,
    private ngZone: NgZone
  ) {}
  ngOnInit(): void {
    // Add Ticket Form Validation And Getting Values
    this.profileSettingsForm = this.formBuilder.group({
      firstName: ['', [Validators.required]],
      email: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      address: ['', [Validators.required]],
      state: ['', [Validators.required]],
      city: ['', [Validators.required]],
      zipCode: ['', [Validators.required]],
    });
  }
}
