import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { NotificationDto } from '../models/notificationDto';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {

  private hubConnection!: signalR.HubConnection;

  //  flux observable vers les composants
  private _notification$ = new BehaviorSubject<NotificationDto | null>(null);
  public notification$ = this._notification$.asObservable();

  //  anti-duplication
  private lastNotificationKey: string | null = null;

  constructor() {
    this.startConnection();
  }


  // SignalR Connection

  private startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7063/notificationHub', {
        accessTokenFactory: () => localStorage.getItem('token') ?? ''
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR connected'))
      .catch(err => console.error('SignalR connection error:', err));

  
    // Receive notification
    
    this.hubConnection.on(
      'ReceiveNotification',
      (data: NotificationDto) => {

        if (!data || !data.title || !data.message) return;

        // clé unique pour éviter doublons
        const key = `${data.title}|${data.message}`;

        if (this.lastNotificationKey === key) return;

        this.lastNotificationKey = key;

        console.log('Notification reçue:', data);

        this._notification$.next({
          title: data.title,
          message: data.message
        });
      }
    );
  }


  //  Stop connection 
  
  stopConnection(): void {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }
}
