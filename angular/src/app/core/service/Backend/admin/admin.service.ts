import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { changePassword } from 'src/app/core/models/Users/chnagePassword';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
private apiUrl =environment.apiUrl+"/admin";
  

  constructor(private http: HttpClient) { }

   // HTTP Options
   private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };
  chnagePassword(user:changePassword): Observable<any> {
      const url = `${this.apiUrl}/changePassword`;
      return this.http.put<any>(url, user, this.httpOptions).pipe(
        catchError(error => {
          if (error.status === 400 ) {
            // Return the error object with the validation errors
            throw error;
          } else {
            let errorMessage = error.error?.message || error.message || 'Unknown error';
            throw new Error(errorMessage);
          }
        })
      );
    }
}
