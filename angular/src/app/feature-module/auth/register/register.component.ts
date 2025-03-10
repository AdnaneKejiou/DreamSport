import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { selectTenantData } from 'src/app/store/tenant/tenant.selectors';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  public routes = routes;
  public CustomControler!: string | number;
  public isValidConfirmPassword = false;
  public show_password1= true;
  public show_password2 = true;
  public show_password3= true;
  public show_password4 = true;
  public confirmPassword = true;
  tenantData$: Observable<any>;
    imageUrl: string | null = null;
    
  form = new FormGroup({
    Username: new FormControl('', [Validators.required]),
    email: new FormControl('', [
      Validators.required,
      Validators.email,
    ]),
    password: new FormControl('', [Validators.required]),
    confirmpassword: new FormControl('', [Validators.required]),
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
  signup() {
   
    if (
      this.form.value.password === this.form.value.confirmpassword &&
      this.form.valid
    ) {
      this.confirmPassword = true;
      this.auth.signup();
    } else{
      this.form.markAllAsTouched();
    }
  }


}
