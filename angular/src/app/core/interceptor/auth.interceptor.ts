import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from '../service/auth/authservice';
import { Router } from '@angular/router';
import { selectTenantId } from '../../core/store/tenant/tenant.selectors';
import { Store} from '@ngrx/store';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;

  Tenant$: Observable<string | number | null>; // Observable for tenantId
  Tenant: string | number | null = null; // Raw value for tenantId
  private excludedRoutes: string[] = [
    '/auth/register',
    '/auth/forgot-password'
  ];

  constructor(private authService: AuthService,private router: Router,private store: Store) {
    this.Tenant$ = this.store.select(selectTenantId);

    // Subscribe to the observable to get the tenantId value
    this.Tenant$.subscribe((tenantId) => {
      this.Tenant = tenantId; // Assign the value to the Tenant property
    });
  } 

  private isExcluded(url: string): boolean {
    return this.excludedRoutes.some((excludedRoute) => url.includes(excludedRoute));
  }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {      
    if (this.isExcluded(req.url)) {
      return next.handle(req); // Skip adding headers for excluded routes
    }
  
    let headers = req.headers;
    const token = this.authService.getAccessToken();

    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }
    if (this.Tenant) {
      headers = headers.set('Tenant-ID', this.Tenant.toString());
    }
    console.error("tenant :    ",this.Tenant);
    const modifiedReq = req.clone({ headers });
    
    return next.handle(modifiedReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && !this.isRefreshing) {
          this.isRefreshing = true;
          return this.authService.refreshToken().pipe(
            switchMap((response: any) => {
              this.isRefreshing = false;
              const newToken = response.token;
              if (newToken) {
                const newHeaders = modifiedReq.headers.set('Authorization', `Bearer ${newToken}`);
                return next.handle(modifiedReq.clone({ headers: newHeaders }));
              }
              return throwError(() => new Error('Token refresh failed'));
            }),
            catchError(() => {
              this.isRefreshing = false;
              this.authService.logout();
              this.router.navigate(['/auth/login']);
              return throwError(() => new Error('Session expired'));
            })
          );
        }
        return throwError(() => error);
      })
    );
  }
  
}
