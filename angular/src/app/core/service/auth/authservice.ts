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

declare var gapi: any;
import { loadGapiInsideDOM } from 'gapi-script';
@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/Login`;
  private accessTokenKey = 'access_token';
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
  private Tenant =11;
  private userKey = 'user_data';
  private GoogleClientId: string = '783212430758-tu7hekssgqrbcmldctchgi7ruasi25rk.apps.googleusercontent.com';

  constructor(private router: Router, private http: HttpClient) {}

  signup(){
    this.router.navigate(['/login']);
  }

  login(email: string, password: string, userType: UserType): Observable<any> {
    const endpoint = environment.apiUrl + LOGIN_ENDPOINTS[userType];  // Ensure this resolves to the correct endpoint
    console.warn("📢 Login endpoint:", endpoint);  // Log the endpoint URL
  
    const body = {
      'email': email,
      'password': password,
      'adminId': this.Tenant,  
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
    }else if( decodedToken && decodedToken.role === UserType.CLIENT){
      this.router.navigate(['/user/user-dashboard']);
    }else if( decodedToken && decodedToken.role === UserType.EMPLOYEE){
      this.router.navigate(['/employee/dashboard']);
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
      'adminId': this.Tenant,  
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

  public initializeGoogleSDK() {
    gapi.load('auth2', () => {
      gapi.auth2.init({
        client_id: this.GoogleClientId
      });
    });
  }

  public GoogleSignIn(): Promise<any> {
    const auth2 = gapi.auth2.getAuthInstance();
    return new Promise((resolve, reject) => {
      auth2.signIn().then((googleUser: any) => {
        const profile = googleUser.getBasicProfile();
        const idToken = googleUser.getAuthResponse().id_token;
        resolve({ idToken, profile });
      }).catch((error: any) => {
        reject(error);
      });
    });
  }
}

