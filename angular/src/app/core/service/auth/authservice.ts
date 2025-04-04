import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { routes } from '../../helpers/routes';
import { HttpClient } from '@angular/common/http';
import { tap, catchError } from 'rxjs/operators';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { UserType, LOGIN_ENDPOINTS } from '../../contantes/UserType';
import { jwtDecode } from 'jwt-decode';
import { DecodedToken } from '../../models/decoded-token.model';
import { EquipeService } from '../equipe/equipe.service';


@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private logoutUrl = environment.apiUrl;
  private apiUrl = `${environment.apiUrl}/Login`;
  private accessTokenKey = 'access_token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  private Tenant =11;
  private userKey = 'user_data';
  private GoogleClientId: string = '39053290852-ol06nn3fdpl6cbs2eobh3toc44dbb8kr.apps.googleusercontent.com';

  constructor(private router: Router, private http: HttpClient) {}

  manualSignup(userData: any): Observable<any> {
    const endpoint= `${environment.apiUrl}/users`;
    console.log(userData);  
    return this.http.post(endpoint, userData).pipe(
      catchError((errorResponse) => {
        // Extract nested "errors" object
        const backendErrors = errorResponse.error?.errors || {};
        return throwError(() => backendErrors);
      })
    ); 
  }
   getUserId(): number {
    const userString = localStorage.getItem('user_data');
    
    if (!userString) {
      console.warn('No user data found in localStorage');
      return 0; // ou throw new Error('User not authenticated');
    }
  
    try {
      const user = JSON.parse(userString);
      
      if (!user || !user.nameid) {
        console.warn('User ID not found in user data');
        return 0;
      }
      
      const userId = Number(user.nameid);
      
      if (isNaN(userId)) {
        console.error('User ID is not a valid number');
        return 0;
      }
      
      return userId;
      
    } catch (error) {
      console.error('Error parsing user data:', error);
      return 0;
    }
  }


  login(email: string, password: string, userType: UserType): Observable<any> {
    const endpoint = environment.apiUrl + LOGIN_ENDPOINTS[userType];  // Ensure this resolves to the correct endpoint
    console.warn("📢 Login endpoint:", endpoint);  // Log the endpoint URL
  
    const body = {
      'email': email,
      'password': password,
      'AdminId': this.Tenant,  
      'facebookToken': null, 
      'googleToken': null 
    };
    const headers = {
      'Tenant-ID': this.Tenant.toString(),
      'Content-Type': 'application/json'
    };
  
    console.log("📢 Sending request with body:", body);  // Log the body being sent
  
    return this.http.post(endpoint, body, { headers, withCredentials: true }).pipe(
      tap((response: any) => {
        this.storeAccessToken(response.token);
        this.isAuthenticatedSubject.next(true);
        this.RedirectDashboard();
      }),
      catchError((error) => {
        console.error("err : ",error);
        return throwError(() => error);
      })
    );
  }
  
  refreshToken(): Observable<any> {
    console.warn("asda");

    return this.http.post(`${this.apiUrl}/RefreshToken`, {}, { withCredentials: true }).pipe(
      tap((response: any) => {
        this.storeAccessToken(response.token);
      }),
      catchError(this.handleError)
    );
  }

  private RedirectDashboard(){
    const userData = localStorage.getItem(this.userKey);
    const decodedToken: DecodedToken | null = userData ? JSON.parse(userData) as DecodedToken : null;
    if(decodedToken && decodedToken.role === UserType.ADMIN){
      this.router.navigate(['/admin/dashboard']);
      return;
    }else if( decodedToken && decodedToken.role === UserType.CLIENT){
      this.router.navigate(['/user/user-dashboard']);
      return;
    }else if( decodedToken && decodedToken.role === UserType.EMPLOYEE){
      this.router.navigate(['/coaches/pages/dashboard']);
      return;
    }
  }
  // ✅ Store only the access token
  private storeAccessToken(accessToken: string): void {
    localStorage.setItem(this.accessTokenKey, accessToken);

    const decoded: DecodedToken = jwtDecode<DecodedToken>(accessToken);
    localStorage.setItem(this.userKey, JSON.stringify(decoded));
  }

  // Get decoded user data from localStorage
  getDecodedToken(): DecodedToken | null {
    const userData = localStorage.getItem(this.userKey);
    return userData ? JSON.parse(userData) : null;
  }
  // ✅ Get Access Token
  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  // ✅ Check if user is authenticated
  isAuthenticated(): Observable<boolean> {
    return this.isAuthenticatedSubject.asObservable();
  }

  private hasToken(): boolean {
    return !!this.getAccessToken();
  }

  // ✅ Logout: Clears access token and triggers backend logout (clears cookie)
  logout(): void {
    localStorage.removeItem(this.accessTokenKey);
    this.isAuthenticatedSubject.next(false);
    this.http.post(`${this.apiUrl}/logout`, {}, { withCredentials: true }).subscribe(() => {
      this.router.navigate([routes.login]);
    });
  }

  // ✅ Handle errors
  private handleError(error: any) {
    console.error('Authentication error:', error);
    return throwError(() => new Error(error.message || 'Server error'));
  }
  
  forgotpassword(){
    return this.http.post(`${this.apiUrl}/forgotpassword`, {}, { withCredentials: true }).pipe
  }

  LoginWithFacebook(credentials: string): Observable<any> {
    const endpoint = environment.apiUrl + LOGIN_ENDPOINTS["User"];
    const body = {
      'email': null,
      'password': null,
      'facebookToken': credentials, 
      'googleToken': null 
    };
    const headers = {
      'Tenant-ID': this.Tenant.toString(),
      'Content-Type': 'application/json'
    };
  
    console.log("📢 Sending request with body:", body);  // Log the body being sent
  
    return this.http.post(endpoint, body, { headers, withCredentials: true }).pipe(
      tap((response: any) => {
        this.storeAccessToken(response.token);
        this.isAuthenticatedSubject.next(true);
        this.RedirectDashboard();
      }),
      catchError((error) => {
        return throwError(() => error);
      })
    );
  }

  initializeGoogleSignIn(): void {
    window.google.accounts.id.initialize({
      client_id: this.GoogleClientId,  // Use the correct Google Client ID
      callback: this.handleCredentialResponse.bind(this) // Bind the context for 'this'
    });
    
  }
  
  GoogleSignIn(): void {
    // Clear any existing login state (if needed)
    window.google.accounts.id.disableAutoSelect();
  
    // Trigger the Google login prompt
    window.google.accounts.id.prompt();
  }

  handleCredentialResponse(response: any): void {
    if (response && response.credential) {
      this.verifyToken(response.credential).subscribe({
        next: (data) => {
          console.log('Successfully logged in with Google');
        },
        error: (err) => {
          console.error('Error during Google login:', err);
        }
      });
    } else {
      console.error('Google login failed: No credential returned');
    }
  }
  verifyToken(token: string) {
    const endpoint = environment.apiUrl + LOGIN_ENDPOINTS["User"];
    const body = {
      'email': null,
      'password': null,
      'facebookToken': null, 
      'googleToken': token 
    };
    const headers = {
      'Tenant-ID': this.Tenant.toString(),
      'Content-Type': 'application/json'
    };
  
    console.log("📢 Sending request with body:", body);  // Log the body being sent
  
    return this.http.post(endpoint, body, { headers, withCredentials: true }).pipe(
      tap((response: any) => {
        this.storeAccessToken(response.token);
        this.isAuthenticatedSubject.next(true);
        this.RedirectDashboard();
      }),
      catchError((error) => {
        return throwError(() => error);
      })
    );
  }

  forgotPassword(email:any , usertype:string): Observable<any> {
    var endpoint = "";
    if(usertype === UserType.ADMIN){
      endpoint = this.logoutUrl + "/admin/recover-password";
    }else if (usertype === UserType.EMPLOYEE){
      endpoint = this.logoutUrl + "/employee/recover-password";
    }else if( usertype === UserType.CLIENT ){
      endpoint = this.logoutUrl + "/users/recover-password";
          console.log("DAS : ",usertype);
   }
   const body = {
    'email': email,
  };

  console.log("📢 Sending request with body:", body);  // Log the body being sent

  return this.http.put(endpoint, body, { withCredentials: true }).pipe(
    tap((response: any) => {
      console.log("success " );
    }),
    catchError((error) => {
      console.error("err : ",error);
      return throwError(() => error);
    })
  );
  }
}










