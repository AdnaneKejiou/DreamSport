import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { SendTeamMessageDTO } from 'src/app/core/models/chat/send-team-message.dto';
import { AmisChatReturnedDTO } from 'src/app/core/models/chat/amis-chat-returned.dto';
import { AmisMessageDTO } from 'src/app/core/models/chat/amis-message.dto';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { EquipeService } from 'src/app/core/service/equipe/equipe.service';
import { ChatAmisService } from 'src/app/core/service/chat/chatamis.service';
import { ChatTeamService } from 'src/app/core/service/chat/chat-team.service';
import { BlockService } from 'src/app/core/service/block/block.service';
import { catchError, map, Observable, of } from 'rxjs';
import { SendAmisMessageDTO } from 'src/app/core/models/chat/send-amis-message.dto';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  public openChat = false;
  public currentUserId: number;
  public teamId!: number;
  public amisChats: AmisChatReturnedDTO[] = [];
  public teamChats: any[] = [];
  public selectedChat: any;
  public messages: AmisMessageDTO[] = [];
  public newMessage = '';
  public loading: boolean = false;
  public blockStatus = {
    iBlockedThem: false,
    theyBlockedMe: false
  };

  constructor(
    private auth: AuthService,
    private equipeService: EquipeService,
    private chatAmisService: ChatAmisService,
    private chatTeamService: ChatTeamService,
    private blockService: BlockService,
    private cdRef: ChangeDetectorRef
  ) {
    this.currentUserId = this.auth.getUserId();
  }

  ngOnInit() {
    this.currentUserId = this.auth.getUserId();
    this.loadTeamId();
    this.loadAmisChats();
  }

  private loadTeamId() {
    this.getTeamId(this.currentUserId).subscribe({
      next: (teamId) => {
        this.teamId = teamId;
        this.loadTeamChats();
      },
      error: (err) => console.error('Error:', err)
    });
  }

  getTeamId(userId: number): Observable<number> {
    return this.equipeService.checkMembership(userId).pipe(
      map(response => response.equipeId),
      catchError(err => {
        console.error('Error checking team status', err);
        return of(0);
      })
    );
  }

  private loadAmisChats() {
    this.chatAmisService.getAmisChatInfo(this.currentUserId).subscribe({
      next: (chats) => {
        this.amisChats = chats;
      },
      error: (err) => console.error('Error:', err)
    });
  }

  private loadTeamChats() {
    if (this.teamId) {
      this.chatTeamService.getTeamChatInfo(this.teamId, this.currentUserId)
        .subscribe({
          next: (chat) => {
            this.teamChats = [{
              ...chat,
              lastMessage: chat.lasteMessage
            }];
          },
          error: (err) => console.error('Error:', err)
        });
    }
  }

  public chatOpen(chat?: any, isTeam: boolean = false) {
    this.openChat = !this.openChat;
    if (chat) {
        this.selectedChat = { ...chat, isTeam };
        this.loadMessages(chat.id);
        this.loadBlockStatus();

        // If it's a team chat, mark all messages as seen
        if (isTeam) {
            this.chatTeamService.markAllAsSeen(this.selectedChat.id, this.currentUserId)
                .subscribe({
                    next: () => {
                        this.updateMessagesStatus(this.messages.map(m => m.id));
                        this.cdRef.detectChanges();
                    },
                    error: (err) => console.error('Error marking team messages as seen:', err)
                });
        }
    } else {
        this.selectedChat = null;
    }
}

  private loadMessages(chatId: number) {
    this.loading = true;
    this.messages = [];
    
    if (this.selectedChat.isTeam) {
      this.chatTeamService.getFullConversation(this.teamId).subscribe({
        next: (messages) => {
          this.messages = messages;
          this.markAllMessagesAsSeen();
          this.loading = false;
          this.scrollToBottom();
        },
        error: (err) => {
          console.error('Error:', err);
          this.loading = false;
        }
      });
    } else {
      this.chatAmisService.getConversation(chatId).subscribe({
        next: (messages) => {
          this.messages = messages;
          this.markAllMessagesAsSeen();
          this.loading = false;
          this.scrollToBottom();
        },
        error: (err) => {
          console.error('Error:', err);
          this.loading = false;
        }
      });
    }
  }

  private markAllMessagesAsSeen() {
    if (!this.selectedChat || this.messages.length === 0) return;

    const unreadMessages = this.messages.filter(m => 
      m.emetteur.id !== this.currentUserId && 
      m.statut !== 'Seen'
    );

    if (unreadMessages.length === 0) return;

    const messageIds = unreadMessages.map(m => m.id);

    if (this.selectedChat.isTeam) {
      this.chatTeamService.markMultipleAsSeen(messageIds, this.currentUserId)
        .subscribe({
          next: () => {
            this.updateMessagesStatus(messageIds);
            this.cdRef.detectChanges();
          },
          error: (err) => console.error('Error marking team messages as seen:', err)
        });
    } else {
      this.chatAmisService.markMultipleAsSeen(messageIds, this.currentUserId)
        .subscribe({
          next: () => {
            this.updateMessagesStatus(messageIds);
            this.cdRef.detectChanges();
          },
          error: (err) => console.error('Error marking friend messages as seen:', err)
        });
    }
  }

  private updateMessagesStatus(messageIds: number[]) {
    this.messages = this.messages.map(message => {
      if (messageIds.includes(message.id)) {
        return { ...message, statut: 'Seen' };
      }
      return message;
    });

    if (this.selectedChat.isTeam) {
      this.teamChats = this.teamChats.map(chat => {
        if (chat.id === this.selectedChat.id) {
          return { ...chat, nbrMessage: 0 };
        }
        return chat;
      });
    } else {
      this.amisChats = this.amisChats.map(chat => {
        if (chat.id === this.selectedChat.id) {
          return { ...chat, unreadCount: 0 };
        }
        return chat;
      });
    }
  }

  private scrollToBottom() {
    setTimeout(() => {
      const container = document.querySelector('.chat-body .chat-scroll');
      if (container) {
        container.scrollTop = container.scrollHeight;
      }
    }, 100);
  }

  private loadBlockStatus() {
    if (this.selectedChat && !this.selectedChat.isTeam) {
      this.blockService.getBlockStatusBetweenUsers(this.currentUserId, this.selectedChat.idMember)
        .subscribe({
          next: (status) => {
            this.blockStatus = {
              iBlockedThem: status.currentUserBlockedTarget,
              theyBlockedMe: status.targetUserBlockedCurrent
            };
            this.cdRef.detectChanges();
          },
          error: (err) => {
            this.blockStatus = { iBlockedThem: false, theyBlockedMe: false };
          }
        });
    } else {
      this.blockStatus = { iBlockedThem: false, theyBlockedMe: false };
    }
  }

  public sendMessage() {
    if (!this.selectedChat || !this.newMessage.trim()) return;
    
    if (!this.selectedChat.isTeam && this.isCommunicationBlocked()) {
      alert('You cannot send messages to this user');
      return;
    }

    if (this.selectedChat.isTeam) {
      const messageDto: SendTeamMessageDTO = {
        teamId: this.teamId,
        emetteurId: this.currentUserId,
        contenu: this.newMessage,
      };

      this.chatTeamService.sendMessage(messageDto).subscribe({
        next: (message) => {
          this.messages.push(message);
          this.newMessage = '';
          this.updateTeamLastMessage(message.contenu);
          this.scrollToBottom();
        },
        error: (err) => console.error('Error:', err)
      });
    } else {
      const messageDto: SendAmisMessageDTO = {
        chatAmisId: this.selectedChat.id,
        emetteurId: this.currentUserId,
        contenu: this.newMessage,
      };

      this.chatAmisService.sendMessage(messageDto).subscribe({
        next: (message) => {
          this.messages.push(message);
          this.newMessage = '';
          this.updateFriendLastMessage(message.contenu);
          this.scrollToBottom();
        },
        error: (err) => console.error('Error:', err)
      });
    }
  }

  private updateFriendLastMessage(message: string) {
    const chat = this.amisChats.find(c => c.id === this.selectedChat.id);
    if (chat) {
      chat.lastMessage = message;
      chat.date = new Date();
      this.cdRef.detectChanges();
    }
  }

  private updateTeamLastMessage(message: string) {
    const chat = this.teamChats.find(c => c.id === this.selectedChat.id);
    if (chat) {
      chat.lastMessage = message;
      chat.date = new Date();
      this.cdRef.detectChanges();
    }
  }

  public blockUser(userId: number) {
    if (!userId || userId === this.currentUserId) return;
    
    this.blockService.blockUser(userId, this.currentUserId).subscribe({
      next: () => {
        this.loadBlockStatus();
        this.loadAmisChats();
        if (this.selectedChat?.idMember === userId) {
          this.messages = [];
        }
      },
      error: (err) => console.error('Error blocking user:', err)
    });
  }

  public unblockUser(userId: number) {
    if (!userId || userId === this.currentUserId) return;
    
    this.blockService.unblockUser(userId, this.currentUserId).subscribe({
      next: () => {
        this.blockStatus.iBlockedThem = false;
        this.loadAmisChats();
        this.loadBlockStatus();
        this.cdRef.detectChanges();
      },
      error: (err) => {
        console.error('Error unblocking user:', err);
        this.blockStatus.iBlockedThem = true;
      }
    });
  }

  public isCommunicationBlocked(): boolean {
    return this.blockStatus.iBlockedThem || this.blockStatus.theyBlockedMe;
  }
}