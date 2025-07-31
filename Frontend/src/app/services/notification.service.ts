import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private hubConnection!: signalR.HubConnection;
  private _notification$ = new BehaviorSubject<string | null>(null);
  public notification$ = this._notification$.asObservable();

  constructor() {
    this.startConnection();
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7063/notificationHub') 
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('✅ SignalR connected'))
      .catch((err) => console.error('❌ SignalR error:', err));

    this.hubConnection.on('ReceiveNotification', (message: string) => {
      console.log('📢 Notification reçue:', message);
      this._notification$.next(message);
    });
  }
}
