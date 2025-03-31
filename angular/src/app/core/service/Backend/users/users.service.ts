import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { userRes } from 'src/app/core/models/Users/userRes';
export interface userBlock{
  id: number,
  firstName: string,
  lastName: string,
  email: string,
  imageUrl: string,
  username: string,
  isBlocked: boolean,
  phoneNumber: string
}
@Injectable({
  providedIn: 'root'
})

export class UsersService {
  private apiUrl =environment.apiUrl+"/users";
  private httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

  constructor(private http: HttpClient) { }

  // Get a single user by ID
    getUser(id: number): Observable<userRes> {
      const url = `${this.apiUrl}/get-right/${id}`;
      return this.http.get<userRes>(url);
    }
    getUsers(skip: number, limit: number, isBlocked?: boolean, searchTerm?: string): Observable<{ users: userBlock[]; totalCount: number }> {
      const url = `${this.apiUrl}/pagination`;
      
      // Construct the request body
      const body: any = {
        skip,
        limit,
      };
    
      // Add optional parameters if defined
      if (isBlocked !== undefined) {
        body.isBlocked = isBlocked;
      }
      if (searchTerm) {
        body.searchTerm = searchTerm;
      }
    
      return this.http.post<{ users: userBlock[]; totalCount: number }>(url, body);
    }

    updateUserStatus(userId: number, isBlocked: boolean): Observable<any> {
      return this.http.put(`${this.apiUrl}/${userId}/status`, { isBlocked });
    }
}

