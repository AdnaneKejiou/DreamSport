import { Component, OnInit , NgZone} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { routes } from 'src/app/core/helpers/routes';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { selectTenantData } from 'src/app/store/tenant/tenant.selectors';
import { UserType } from '../../../core/contantes/UserType';
import { Router } from '@angular/router';


declare const FB: any;
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

  constructor(private auth: AuthService ,private store: Store,private _ngZone: NgZone, private router: Router) {
    this.tenantData$ = this.store.select(selectTenantData);
    this.tenantData$.subscribe(data => {
      if (data && data.siteInfo && data.siteInfo.length > 0) {
        this.imageUrl = data.siteInfo[0].logo;
      }
    });     
  }

  user() {
    if (this.form.valid) {
      const email = this.form.value.email || '';
      const password = this.form.value.password || '';
      this.auth.login(email, password, UserType.CLIENT).subscribe(
        (response) => {
          console.log("Login Successful", response);
        },
        (error) => {
          console.error("Login Failed", error);
        }
      );;  // Example API call
    } else {
      this.form.markAllAsTouched();
    }
  }
  
  admin() {
    if (this.form.valid) {
      const email = this.form.value.email || '';
      const password = this.form.value.password || '';
      this.auth.login(email, password, UserType.ADMIN); // Example API call
    } else {
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
    }// need to be removed
    this.auth.initializeGoogleSDK();
  }

 
  async facebookLogin() {
    console.warn("haha")
    FB.login(async (response: any) => {
      if (response.authResponse) {
        this.auth.LoginWithFacebook(response.authResponse.accessToken)
          .subscribe(
            (res: any) => {
              this._ngZone.run(() => {
                console.warn(response.authResponse);
                
              });
            },
            (error: any) => {
              console.error('Facebook login error:', error);
            }
          );
      } else {
        console.error('User canceled login or did not fully authorize.');
      }
    }, { scope: 'email' });
  }
  
  Googlelogin() {
    this.auth.GoogleSignIn().then((data) => {
      console.log('Google user info:', data);
      // Here you can send the idToken to your server to authenticate
    }).catch((error) => {
      console.error('Login failed', error);
    });
  }
}
