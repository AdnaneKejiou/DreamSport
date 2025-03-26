// src/app/core/services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface User {
  id: number;
  nom: string;
  prenom: string;
  username: string | null;
  email: string;
  imageUrl: string | null;
  bio: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5010/gateway/users/search';

  constructor(private http: HttpClient) { }

  searchUsers(query: string): Observable<any[]> {
    // Encoder le query pour les espaces et caractères spéciaux
    const encodedQuery = encodeURIComponent(query);
    return this.http.get<any[]>(`${this.apiUrl}/${encodedQuery}`);
  }

  sendInvitation(userId: number): Observable<any> {
    // Implémentez cette méthode selon votre API d'envoi d'invitations
    // return this.http.post('/api/invitations', { userId });
    return new Observable(observer => {
      observer.next({ success: true });
      observer.complete();
    });
  }
}