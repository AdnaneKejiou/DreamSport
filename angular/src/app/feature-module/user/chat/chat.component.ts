import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { SendTeamMessageDTO } from 'src/app/core/models/chat/send-team-message.dto';
import { AuthService } from 'src/app/core/service/auth/authservice';
import { catchError, map, Observable, of, Subscription } from 'rxjs';
import { AmisChatReturnedDTO } from 'src/app/core/models/chat/amis-chat-returned.dto';
import { TeamChatReturnedDTO } from 'src/app/core/models/chat/team-chat-returned.dto';
import { AmisMessageDTO } from 'src/app/core/models/chat/amis-message.dto';
import { TeamMessageDTO } from 'src/app/core/models/chat/team-message.dto';
import { EquipeService } from 'src/app/core/service/equipe/equipe.service';
import { ChatAmisService } from 'src/app/core/service/chat/chatamis.service';
import { ChatTeamService } from 'src/app/core/service/chat/chat-team.service';
import { BlockService } from 'src/app/core/service/block/block.service';
import { ChatSignalRService } from 'src/app/core/service/chatsignalr/chatsignalr.service';
import { SendAmisMessageDTO } from 'src/app/core/models/chat/send-amis-message.dto';
import { User, UserService } from 'src/app/core/service/user/user.service';

interface SelectedChat {
  chat: AmisChatReturnedDTO | TeamChatReturnedDTO;
  isTeam: boolean;
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatComponent implements OnInit, OnDestroy {
  public openChat = false;
  public currentUserId: number;
  public teamId: number = 0;
  public amisChats: AmisChatReturnedDTO[] = [];
  public teamChats: TeamChatReturnedDTO[] = [];
  public selectedChat: SelectedChat | null = null;
  public messages: (AmisMessageDTO | TeamMessageDTO)[] = [];
  public newMessage = '';
  public loading = false;
  public initialLoad = true;
  public blockStatus = { iBlockedThem: false, theyBlockedMe: false };
  public isTyping = false;
  public typingUserId: number | null = null;
  public typingUserName: string = '';
  
  private scrollContainer: HTMLElement | null = null;
  private subscriptions: Subscription = new Subscription();
  private typingTimeout: any;
  private typingDebounceTimeout: any;
  private receiverId: number = 0;
  private processedMessageIds = new Set<number>();

  constructor(
    private auth: AuthService,
    private equipeService: EquipeService,
    private chatAmisService: ChatAmisService,
    private chatTeamService: ChatTeamService,
    private blockService: BlockService,
    private cdRef: ChangeDetectorRef,
    private chatSignalRService: ChatSignalRService,
    private user : UserService
  ) {
    this.currentUserId = this.auth.getUserId();
  }

  ngOnInit() {
    this.currentUserId = this.auth.getUserId();
    this.loadTeamId();
    this.loadAmisChats();
    this.setupSignalRListeners();
  }

  ngOnDestroy() {
    this.cleanup();
  }

  private cleanup(): void {
    if (this.scrollContainer) {
      this.scrollContainer.removeEventListener('scroll', this.handleScroll.bind(this));
    }
    if (this.typingTimeout) {
      clearTimeout(this.typingTimeout);
    }
    if (this.typingDebounceTimeout) {
      clearTimeout(this.typingDebounceTimeout);
    }
    this.subscriptions.unsubscribe();
    this.processedMessageIds.clear();
  }

  private setupSignalRListeners(): void {
    this.subscriptions.add(
      this.chatSignalRService.amisMessageReceived.subscribe(message => {
        this.handleIncomingAmisMessage(message);
      })
    );

    this.subscriptions.add(
      this.chatSignalRService.teamMessageReceived.subscribe(message => {
        this.handleIncomingTeamMessage(message);
      })
    );

    this.subscriptions.add(
      this.chatSignalRService.messageSeenUpdate.subscribe(({messageIds, seenByUserId}) => {
        if (seenByUserId !== this.currentUserId) {
          this.updateMessagesStatus(messageIds);
        }
      })
    );

    this.subscriptions.add(
      this.chatSignalRService.typingStatusUpdate.subscribe(({userId, isTyping, isTeamChat, chatId,username}) => {
        this.handleTypingStatus(userId, isTyping, isTeamChat, chatId,username);
      })
    );
  }

  private handleTypingStatus(userId: number, isTyping: boolean, isTeamChat: boolean, chatId: number , UserName?:string): void {
    if (userId === this.currentUserId) return;

    if ((this.selectedChat?.isTeam === isTeamChat) && 
        (this.selectedChat?.chat.id === chatId)) {
      this.isTyping = isTyping;
      this.typingUserId = userId;

      if (isTyping) {
        if (isTeamChat) {
          const teamChat = this.teamChats.find(c => c.id === chatId);
          if (teamChat) {
            if(UserName){
              this.typingUserName =UserName;
            }else{
              this.typingUserName =teamChat.equipeName;

            }
          }
        } else {
          const amisChat = this.amisChats.find(c => c.id === chatId);
          if (amisChat) {
            this.typingUserName = amisChat.amiName;
          }
        }

        if (this.typingTimeout) {
          clearTimeout(this.typingTimeout);
        }
       this.scrollToBottom();
        this.typingTimeout = setTimeout(() => {
          this.isTyping = false;
          this.typingUserId = null;
          this.cdRef.detectChanges();
        }, 3000);
      }
      this.cdRef.detectChanges();
    }
  }

  public onInputChange(): void {
    if (!this.selectedChat) return;

    if (this.typingDebounceTimeout) {
      clearTimeout(this.typingDebounceTimeout);
    }

 
      
      this.typingUserName=this.auth.getUserName();


    this.chatSignalRService.notifyTyping(
      true, 
      this.selectedChat.isTeam, 
      this.selectedChat.chat.id, 
      this.selectedChat.isTeam ? 0 : parseInt((this.selectedChat.chat as AmisChatReturnedDTO).idMember),
      this.typingUserName

      
    ).catch(err => console.error('Error notifying typing:', err));

    this.typingDebounceTimeout = setTimeout(() => {
      this.chatSignalRService.notifyTyping(
        false, 
        this.selectedChat?.isTeam || false, 
        this.selectedChat?.chat.id || 0, 
        this.selectedChat?.isTeam ? 0 : parseInt((this.selectedChat?.chat as AmisChatReturnedDTO).idMember || '0'),
        this.selectedChat?.isTeam ? "" : this.typingUserName

      ).catch(err => console.error('Error notifying stop typing:', err));
    }, 1000);
   
  }

  private handleScroll(): void {
    this.markVisibleMessagesAsSeen();
  }

  private setupScrollListener(): void {
    this.scrollContainer = document.querySelector('.chat-body .chat-scroll');
    if (this.scrollContainer) {
      this.scrollContainer.removeEventListener('scroll', this.handleScroll.bind(this));
      this.scrollContainer.addEventListener('scroll', this.handleScroll.bind(this));
    }
  }

  private markVisibleMessagesAsSeen(): void {
    if (!this.selectedChat || !this.scrollContainer) return;

    const containerTop = this.scrollContainer.scrollTop;
    const containerBottom = containerTop + this.scrollContainer.clientHeight;
    const unreadMessages: number[] = [];

    this.messages.forEach(message => {
      const element = document.getElementById(`message-${message.id}`);
      if (element) {
        const elementTop = element.offsetTop;
        const elementBottom = elementTop + element.offsetHeight;
        const isVisible = elementBottom >= containerTop && elementTop <= containerBottom;
        
        if (isVisible && 
            message.emetteur.id !== this.currentUserId && 
            message.statut !== 'Seen') {
          unreadMessages.push(message.id);
        }
      }
    });

    if (unreadMessages.length > 0) {
      this.markMessageAsSeen(unreadMessages);
    }
  }

  private scrollToBottom(force: boolean = false): void {
    setTimeout(() => {
      if (this.scrollContainer) {
        const shouldScroll = force || 
                         (this.scrollContainer.scrollTop + this.scrollContainer.clientHeight + 100 > 
                          this.scrollContainer.scrollHeight);
        
        if (shouldScroll) {
          this.scrollContainer.scroll({
            top: this.scrollContainer.scrollHeight,
            behavior: 'smooth'
          });
        }
      }
    }, 100);
  }

  private loadTeamId(): void {
    this.subscriptions.add(
      this.getTeamId(this.currentUserId).subscribe({
        next: (teamId) => {
          this.teamId = teamId;
          this.loadTeamChats();
          this.cdRef.detectChanges();
        },
        error: (err: any) => console.error('Error loading team ID:', err)
      })
    );
  }

  private getTeamId(userId: number): Observable<number> {
    return this.equipeService.checkMembership(userId).pipe(
      map(response => response.equipeId),
      catchError(err => {
        console.error('Error checking team status', err);
        return of(0);
      })
    );
  }

  private loadAmisChats(): void {
    this.subscriptions.add(
      this.chatAmisService.getAmisChatInfo(this.currentUserId).pipe(
        catchError(err => {
          console.error('Error loading friend chats:', err);
          return of([]);
        })
      ).subscribe(chats => {
        this.amisChats = chats;
        this.cdRef.detectChanges();
      })
    );
  }

  private loadTeamChats(): void {
    if (this.teamId) {
      this.subscriptions.add(
        this.chatTeamService.getTeamChatInfo(this.teamId, this.currentUserId).pipe(
          catchError(err => {
            console.error('Error loading team chats:', err);
            return of(null);
          })
        ).subscribe(chat => {
          if (chat) {
            this.teamChats = [chat];
            this.chatSignalRService.joinTeamGroup(chat.id)
              .then(() => console.log('Successfully joined team group'))
              .catch(err => console.error('Error joining team group:', err));
            this.cdRef.detectChanges();
          }
        })
      );
    }
  }

  public chatOpen(chat?: AmisChatReturnedDTO | TeamChatReturnedDTO | null): void {
    this.openChat = !this.openChat;
    if (chat) {
      const isTeam = 'equipeName' in chat;
      this.selectedChat = { chat, isTeam };
      this.loadMessages(chat.id, isTeam);
      if (!isTeam) {
        const amisChat = chat as AmisChatReturnedDTO;
        this.loadBlockStatus(amisChat.idMember);
        this.receiverId = parseInt(amisChat.idMember);
      } else {
        this.receiverId = 0;
      }
    } else {
      this.selectedChat = null;
      this.messages = [];
    }
    this.cdRef.detectChanges();
  }

  private loadMessages(chatId: number, isTeam: boolean = false): void {
    this.loading = true;
    this.initialLoad = true;
    this.messages = [];
    this.cdRef.detectChanges();
    
    const service = isTeam ? this.chatTeamService : this.chatAmisService;
    const method = isTeam ? 'getFullConversation' : 'getConversation';
    const chatid = isTeam ? this.teamId : chatId;
    
    this.subscriptions.add(
      (service as any)[method](chatid).pipe(
        catchError(err => {
          console.error('Error loading messages:', err);
          this.loading = false;
          this.initialLoad = false;
          this.cdRef.detectChanges();
          return of([]);
        })
      ).subscribe({
        next: (messages: any[]) => {
          this.messages = messages;
          this.loading = false;
          this.initialLoad = false;
          this.setupScrollListener();
          this.scrollToBottom(true);
          this.markVisibleMessagesAsSeen();
          this.cdRef.detectChanges();
        }
      })
    );
  }

  private handleIncomingAmisMessage(message: AmisMessageDTO): void {
    if (this.processedMessageIds.has(message.id)) return;
    this.processedMessageIds.add(message.id);

    this.updateAmisChatListWithoutCounter(message);

    if (this.selectedChat?.isTeam === false && this.selectedChat.chat?.id === message.chatAmisId) {
      this.addMessageToChat(message);
      
      if (message.emetteur.id !== this.currentUserId) {
        this.markMessageAsSeen([message.id]);
      }
    } else {
      const chat = this.amisChats.find(c => c.id === message.chatAmisId);
      if (chat) {
        chat.unreadCount = (chat.unreadCount || 0) + 1;
        this.cdRef.detectChanges();
      }
    }
  }

  private handleIncomingTeamMessage(message: TeamMessageDTO): void {
    if (this.processedMessageIds.has(message.id)) return;
    this.processedMessageIds.add(message.id);

    this.updateTeamChatListWithoutCounter(message);

    if (this.selectedChat?.isTeam === true && this.selectedChat.chat?.id === message.chatTeamId) {
      this.addMessageToChat(message);
      
      if (message.emetteur.id !== this.currentUserId) {
        this.markMessageAsSeen([message.id]);
      }
    } else {
      const chat = this.teamChats.find(c => c.id === message.chatTeamId);
      if (chat) {
        chat.nbrMessage = (chat.nbrMessage || 0) + 1;
        this.cdRef.detectChanges();
      }
    }
  }

  private addMessageToChat(message: AmisMessageDTO | TeamMessageDTO): void {
    const messageExists = this.messages.some(m => m.id === message.id);
    if (!messageExists) {
      this.messages = [...this.messages, message];
      this.scrollToBottom();
      this.cdRef.detectChanges();
    }
  }

  private markMessageAsSeen(messageIds: number[]): void {
    if (!this.selectedChat || messageIds.length === 0) return;

    const service = this.selectedChat.isTeam ? this.chatTeamService : this.chatAmisService;
    
    this.subscriptions.add(
      service.markMultipleAsSeen(messageIds, this.currentUserId).subscribe({
        next: () => {
          this.updateMessagesStatus(messageIds);
          this.chatSignalRService.markMessagesAsSeen(messageIds, this.currentUserId)
            .catch(err => console.error('Error notifying message seen:', err));
          this.updateUnreadCount();
        },
        error: (err) => console.error('Error marking messages as seen:', err)
      })
    );
  }

  private updateMessagesStatus(messageIds: number[]): void {
    this.messages = this.messages.map(message => 
      messageIds.includes(message.id) 
        ? { ...message, statut: 'Seen' } 
        : message
    );
    this.cdRef.detectChanges();
  }

  private updateUnreadCount(): void {
    if (!this.selectedChat) return;
    
    if (this.selectedChat.isTeam) {
      this.teamChats = this.teamChats.map(item => 
        item.id === this.selectedChat?.chat.id 
          ? { ...item, nbrMessage: 0 } 
          : item
      );
    } else {
      this.amisChats = this.amisChats.map(item => 
        item.id === this.selectedChat?.chat.id 
          ? { ...item, unreadCount: 0 } 
          : item
      );
    }
    this.cdRef.detectChanges();
  }

  private updateAmisChatListWithoutCounter(message: AmisMessageDTO): void {
    this.amisChats = this.amisChats.map(chat => 
      chat.id === message.chatAmisId
        ? { 
            ...chat, 
            lastMessage: message.contenu,
            date: message.dateEnvoi
          }
        : chat
    );
    this.cdRef.detectChanges();
  }

  private updateTeamChatListWithoutCounter(message: TeamMessageDTO): void {
    this.teamChats = this.teamChats.map(chat => 
      chat.id === message.chatTeamId
        ? { 
            ...chat, 
            lasteMessage: message.contenu,
            date: message.dateEnvoi
          }
        : chat
    );
    this.cdRef.detectChanges();
  }

  public sendMessage(): void {
    if (!this.selectedChat || !this.newMessage.trim() || 
        (!this.selectedChat.isTeam && this.isCommunicationBlocked())) {
      return;
    }

    this.chatSignalRService.notifyTyping(
      false, 
      this.selectedChat.isTeam, 
      this.selectedChat.chat.id, 
      this.selectedChat.isTeam ? 0 : parseInt((this.selectedChat.chat as AmisChatReturnedDTO).idMember)
    ).catch(err => console.error('Error notifying stop typing:', err));

    if (this.selectedChat.isTeam) {
      const messageDto: SendTeamMessageDTO = {
        teamId: this.teamId,
        emetteurId: this.currentUserId,
        contenu: this.newMessage
      };

      this.subscriptions.add(
        this.chatTeamService.sendMessage(messageDto).subscribe({
          next: (message) => {
            this.newMessage = '';
            this.addMessageToChat(message);
            this.updateTeamChatListWithoutCounter(message);
            this.cdRef.detectChanges();
          },
          error: (err: any) => console.error('Error sending message:', err)
        })
      );
    } else {
      const messageDto: SendAmisMessageDTO = {
        chatAmisId: this.selectedChat.chat.id,
        emetteurId: this.currentUserId,
        contenu: this.newMessage
      };

      this.subscriptions.add(
        this.chatAmisService.sendMessage(messageDto).subscribe({
          next: (message) => {
            this.newMessage = '';
            this.addMessageToChat(message);
            this.updateAmisChatListWithoutCounter(message);
            this.cdRef.detectChanges();
          },
          error: (err: any) => console.error('Error sending message:', err)
        })
      );
    }

    this.scrollToBottom();
  }

  public blockUser(userId: string): void {
    const targetUserId = parseInt(userId, 10);
    if (isNaN(targetUserId)) return;
    
    this.subscriptions.add(
      this.blockService.blockUser(targetUserId, this.currentUserId).subscribe({
        next: () => {
          this.loadBlockStatus(userId);
          this.loadAmisChats();
          if (this.selectedChat?.isTeam === false && 
              (this.selectedChat.chat as AmisChatReturnedDTO).idMember === userId) {
            this.messages = [];
          }
          this.cdRef.detectChanges();
        },
        error: (err: any) => console.error('Error blocking user:', err)
      })
    );
  }

  public unblockUser(userId: string): void {
    const targetUserId = parseInt(userId, 10);
    if (isNaN(targetUserId)) return;
    
    this.subscriptions.add(
      this.blockService.unblockUser(targetUserId, this.currentUserId).subscribe({
        next: () => {
          this.blockStatus.iBlockedThem = false;
          this.loadAmisChats();
          this.loadBlockStatus(userId);
          this.cdRef.detectChanges();
        },
        error: (err: any) => {
          console.error('Error unblocking user:', err);
          this.blockStatus.iBlockedThem = true;
          this.cdRef.detectChanges();
        }
      })
    );
  }

  private loadBlockStatus(memberId: string): void {
    if (!memberId) return;
    
    const targetUserId = parseInt(memberId, 10);
    if (isNaN(targetUserId)) return;
  
    this.subscriptions.add(
      this.blockService.getBlockStatusBetweenUsers(this.currentUserId, targetUserId).subscribe({
        next: (status) => {
          this.blockStatus = {
            iBlockedThem: status.currentUserBlockedTarget,
            theyBlockedMe: status.targetUserBlockedCurrent
          };
          this.cdRef.detectChanges();
        },
        error: (err: any) => {
          console.error('Error loading block status:', err);
          this.blockStatus = { iBlockedThem: false, theyBlockedMe: false };
          this.cdRef.detectChanges();
        }
      })
    );
  }

  public isCommunicationBlocked(): boolean {
    return this.blockStatus.iBlockedThem || this.blockStatus.theyBlockedMe;
  }

  public isActiveAmisChat(chat: AmisChatReturnedDTO): boolean {
    return !!this.selectedChat && 
           this.selectedChat.chat?.id === chat.id && 
           !this.selectedChat.isTeam;
  }

  public isActiveTeamChat(chat: TeamChatReturnedDTO): boolean {
    return !!this.selectedChat && 
           this.selectedChat.chat?.id === chat.id && 
           this.selectedChat.isTeam;
  }

  public getMessageSenderId(message: AmisMessageDTO | TeamMessageDTO): number {
    return message.emetteur.id;
  }

  public getMessageSenderAvatar(message: AmisMessageDTO | TeamMessageDTO): string {
    return message.emetteur.avatar || 'assets/img/default-avatar.png';
  }

  public getSelectedChatName(): string {
    if (!this.selectedChat) return '';
    return this.selectedChat.isTeam 
      ? (this.selectedChat.chat as TeamChatReturnedDTO).equipeName
      : (this.selectedChat.chat as AmisChatReturnedDTO).amiName;
  }

  public getSelectedChatAvatar(): string {
    return this.selectedChat?.chat?.avatar || 'assets/img/default-avatar.png';
  }

  public getSelectedChatIdMember(): string | undefined {
    return this.selectedChat?.isTeam 
      ? undefined 
      : (this.selectedChat?.chat as AmisChatReturnedDTO)?.idMember;
  }
}