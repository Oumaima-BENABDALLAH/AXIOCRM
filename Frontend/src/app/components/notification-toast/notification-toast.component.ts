import { Component, OnInit } from '@angular/core';
import { NotificationService } from '../../services/notification.service';
import { NotificationDto } from '../../models/notificationDto';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-notification-toast',
  templateUrl: './notification-toast.component.html',
  styleUrls: ['./notification-toast.component.css']
})
export class NotificationToastComponent implements OnInit {

  toasts: NotificationDto[] = [];
  currentUrl = '';
  todayDay = new Date().getDate(); 
  constructor(
    private notificationService: NotificationService,
    private router: Router
  ) {}

ngOnInit(): void {

  this.router.events
    .pipe(filter(e => e instanceof NavigationEnd))
    .subscribe((e: any) => {
      this.currentUrl = e.urlAfterRedirects;
    });
  this.notificationService.notification$.subscribe(data => {
    if (!data) return;
  if (!this.isOnSchedulerPage()) return;
    this.toasts.push(data);
  });
}
private isOnSchedulerPage(): boolean {
  return this.currentUrl.startsWith('/scheduler');
}
  close(index: number) {
    this.toasts.splice(index, 1);
  }

  snooze(toast: NotificationDto) {
    const index = this.toasts.indexOf(toast);
    this.close(index);

    setTimeout(() => {
      this.toasts.push(toast);
    }, 5 * 60 * 1000); 
  }
}
