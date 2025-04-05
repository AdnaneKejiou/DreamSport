export interface AmisChatReturnedDTO {
    id: number;
    amiName: string;
    lastMessage: string;
    date: Date;
    unreadCount?: number;
    statut?: string;
    avatar: string;
    idMember:string
  }