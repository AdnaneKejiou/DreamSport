import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { selectTenantData } from 'src/app/store/tenant/tenant.selectors';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  public routes = routes;
  public show_password = true;
  public show_password1 =true

  tenantData$: Observable<any>;
  imageUrl: string | null = null;

 

  form = new FormGroup({
    email: new FormControl('', [
      Validators.email,
      Validators.required,
    ]),
    password: new FormControl('', [Validators.required]),
  });

  get f() {
    return this.form.controls;
  }

  constructor(private auth: AuthService ,private store: Store) {
    this.tenantData$ = this.store.select(selectTenantData);
    this.tenantData$.subscribe(data => {
      if (data && data.siteInfo && data.siteInfo.length > 0) {
        this.imageUrl = data.siteInfo[0].logo;
      }
    });     
  }

  user() {
    if (this.form.valid) {
      this.auth.userdashboard();
    }else{
      this.form.markAllAsTouched();
    }
  }
  admin() {
    if (this.form.valid) {
      this.auth.signin();
    }else{
      this.form.markAllAsTouched();
    }
  }
  togglePassword() {
    this.show_password = !this.show_password;
    this.show_password1 =!this.show_password1;
  }
  ngOnInit(): void {
    if (localStorage.getItem('authenticated')) {
      localStorage.removeItem('authenticated');
    }
  }
}
