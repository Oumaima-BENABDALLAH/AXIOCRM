import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { NotificationDto } from '../models/notificationDto';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {

  private hubConnection!: signalR.HubConnection;

  private _notification$ = new BehaviorSubject<NotificationDto | null>(null);
  public notification$ = this._notification$.asObservable();

  constructor() {
    console.log('📦 NotificationService constructor');
    this.startConnection();
  }

  private startConnection(): void {

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7063/notificationHub', {
        accessTokenFactory: () => localStorage.getItem('token') ?? ''
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('✅ SignalR connected'))
      .catch(err => console.error('SignalR connection error:', err));

    this.hubConnection.on('ReceiveNotification', (data: NotificationDto) => {
      console.log('ReceiveNotification TRIGGERED', data);
      this._notification$.next(data);
    });
  }

  stopConnection(): void {
    this.hubConnection?.stop();
  }
}
