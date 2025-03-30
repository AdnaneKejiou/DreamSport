import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { MemberInvitationDTOO } from 'src/app/core/models/member-invitation-dto';
import { Subject } from 'rxjs';
import { AuthService } from '../auth/authservice';
import { UserService } from '../user/user.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  public invitationReceived = new Subject<MemberInvitationDTOO>();
  private isConnected = false;

  constructor(
    private auth: AuthService,
    private userService: UserService
  ) {
    this.buildConnection();
    this.startConnection();
  }

  private buildConnection(): void {
    const userId = this.auth.getUserId().toString();
    
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`http://localhost:5270/invitationHub?userId=${userId}`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .build();
  }

  private async startConnection(): Promise<void> {
    try {
      await this.hubConnection.start();
      this.isConnected = true;
      console.log('SignalR connection established');
      this.registerSignalEvents();
    } catch (err) {
      console.error('Error starting connection:', err);
      setTimeout(() => this.startConnection(), 5000); // Reconnexion après 5s
    }
  }

  private registerSignalEvents(): void {
    this.hubConnection.on('ReceiveInvitation', (invitation: any) => {
      console.log('SignalR - Raw invitation:', invitation);
      
      // Corrigez le nom de la propriété ici
      const correctedInvitation = {
        ...invitation,
        recepteur: invitation.Recerpteur // Mappez Recerpteur vers recepteur
      };
  
      this.userService.getUser(invitation.Emetteur).subscribe({
        next: (sender) => {
          const completeInvitation: MemberInvitationDTOO = {
            id: correctedInvitation.Id,
            emetteur: {
              id: sender.id,
              firstName: sender.prenom || 'Unknown',
              lastName: sender.nom || 'User',
              username: sender.username || '',
              imageUrl: sender.imageUrl || 'https://pub-ae615910610b409dbb3d91c073aa47e6.r2.dev/avatar-02.jpg',
              bio:sender.bio ||''
            },
            recepteur: correctedInvitation.recepteur, // Utilisez la valeur corrigée
            adminId: correctedInvitation.AdminId,
            dateCreated: new Date()
          };
          console.log('SignalR - Processed invitation:', completeInvitation);
          this.invitationReceived.next(completeInvitation);
        },
        error: () => {
          const fallbackInvitation = this.createFallbackInvitation(correctedInvitation);
          this.invitationReceived.next(fallbackInvitation);
        }
      });
    });
  }

 

  private createFallbackInvitation(invitation: any): MemberInvitationDTOO {
    return {
      id: invitation.Id,
      emetteur: {
        id: invitation.Emetteur,
        firstName: 'Unknown',
        lastName: 'User',
        username: '',
        imageUrl: 'assets/img/default-avatar.png',
        bio:'',
      },
      recepteur: invitation.Recepteur,
      dateCreated: new Date()
    };
  }

  public getConnectionStatus(): boolean {
    return this.isConnected;
  }
}