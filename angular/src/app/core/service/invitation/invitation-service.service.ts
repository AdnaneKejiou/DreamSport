import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AddInvMemberDto } from 'src/app/core/models/add-inv-member-dto';
import { MemberInvitationDTOO } from 'src/app/core/models/member-invitation-dto';
import { catchError, forkJoin, map, Observable, of, switchMap, throwError } from 'rxjs';
import { UserService } from '../user/user.service';
@Injectable({
  providedIn: 'root'
})
export class InvitationService {
  private baseUrl = `${environment.apiUrl}/InvitationMember`;

  constructor(private http: HttpClient , private userService:UserService) { }

  sendInvitation(invitation: AddInvMemberDto): Observable<any> {
    return this.http.post(`${this.baseUrl}/send`, invitation);
  }

  acceptInvitation(invitationId: number): Observable<any> {
    return this.http.post(
      `${this.baseUrl}/Accepter/${invitationId}`,
      {},
      { observe: 'response' }
    ).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'An unknown error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side error
      if (error.status === 404) {
        errorMessage = 'Invitation not found';
      } else if (error.status === 400) {
        errorMessage = 'Invalid request';
      } else if (error.status >= 500) {
        errorMessage = 'Server error occurred';
      }
      
      // Use server message if available
      if (error.error?.message) {
        errorMessage = error.error.message;
      }
    }
    
    return throwError(errorMessage);
  }

  refuseInvitation(invitationId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Refuser/${invitationId}`);
  }

  // Dans votre InvitationService
  getUserInvitations(userId: number): Observable<{invitations: MemberInvitationDTOO[], totalData: number}> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Tenant-ID': 28 
    });
    
    return this.http.get<{invitations: any[], totalData: number}>(`${this.baseUrl}/user-invitations/${userId}`, { headers })
      .pipe(
        switchMap(response => {
          // Pour chaque invitation, récupérer les infos de l'émetteur
          const invitationsWithDetails$ = response.invitations.map(invitation => 
            this.userService.getUser(invitation.emetteur).pipe(
              map(user => ({
                ...invitation,
                emetteur: {
                  id: user.id,
                  firstName: user.prenom || 'Unknown',
                  lastName: user.nom || 'User',
                  username: user.username || '',
                  imageUrl: user.imageUrl || 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg',
                  bio:user.bio 
                }
              } as MemberInvitationDTOO)),
              catchError(() => of({
                ...invitation,
                emetteur: {
                  id: invitation.emetteur,
                  firstName: 'Unknown',
                  lastName: 'User',
                  username: '',
                  imageUrl: 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg'
                }
              } as MemberInvitationDTOO))
            )
          );
          
          return forkJoin(invitationsWithDetails$).pipe(
            map(invitations => ({
              invitations,
              totalData: response.totalData
            }))
          );
        })
      );
  }

  getInvitationById(id: number): Observable<MemberInvitationDTOO> {
    return this.http.get<MemberInvitationDTOO>(`${this.baseUrl}/Get/${id}`);
  }
}