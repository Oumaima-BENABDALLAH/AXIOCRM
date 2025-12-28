export interface EmailHistoryGroup {
  date: string; 
  emails: EmailHistoryItem[];
}

export interface EmailHistoryItem {
  toEmail: string;
  subject: string;
  sentAt: string;
  eventTitle: string;
}