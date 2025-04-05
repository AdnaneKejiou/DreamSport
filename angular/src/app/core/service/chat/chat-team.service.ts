import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TeamChatReturnedDTO } from '../../models/chat/team-chat-returned.dto';
import { TeamMessageDTO } from '../../models/chat/team-message.dto';
import { SendTeamMessageDTO } from '../../models/chat/send-team-message.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatTeamService {
  private baseUrl = 'http://localhost:5010/gateway/chatteam';

  constructor(private http: HttpClient) {}

  getTeamChatInfo(teamId: number, memberId: number): Observable<TeamChatReturnedDTO> {
    return this.http.get<TeamChatReturnedDTO>(
      `${this.baseUrl}/${teamId}/members/${memberId}`
    );
  }

  getFullConversation(teamId: number): Observable<TeamMessageDTO[]> {
    return this.http.get<TeamMessageDTO[]>(
      `${this.baseUrl}/${teamId}/conversation`
    );
  }

  markMultipleAsSeen(messageIds: number[], userId: number): Observable<void> {
    const request = {
      messageIds: messageIds,
      userId: userId
    };
  
    return this.http.post<void>(
      `${this.baseUrl}/mark-as-seen`,
      request
    );
  }

  sendMessage(messageDto: SendTeamMessageDTO): Observable<TeamMessageDTO> {
    return this.http.post<TeamMessageDTO>(`${this.baseUrl}/send`, messageDto);
  }

  markAllAsSeen(teamChatId: number, userId: number): Observable<void> {
    const request = {
      teamChatId: teamChatId,
      userId: userId
    };
    return this.http.post<void>(
        `${this.baseUrl}/mark-all-as-seen`,
        request
    );
}
}