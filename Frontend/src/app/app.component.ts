import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { FullCalendarModule } from '@fullcalendar/angular';
import { CalendarOptions } from '@fullcalendar/core'; 
import dayGridPlugin from '@fullcalendar/daygrid';
import { NotificationService } from './services/notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ProductApp';
  showSidebar = true;
  constructor(private router: Router ,  private notificationService: NotificationService) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      const urlPath = event.urlAfterRedirects.split('?')[0];
      console.log("Route actuelle :", event.urlAfterRedirects);
        this.showSidebar = urlPath !== '/login' && urlPath !== '/signup' && urlPath !== '/reset-password' && urlPath !== '/forgot-password';

    });
  }
   calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    plugins: [dayGridPlugin]
  };
}
