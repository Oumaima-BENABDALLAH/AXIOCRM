import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-notification-toast',
  templateUrl: './notification-toast.component.html',
  styleUrls: ['./notification-toast.component.css']
})
export class NotificationCardComponent {
  @Input() title: string = 'Notification';
  @Input() caption: string = '';
}
