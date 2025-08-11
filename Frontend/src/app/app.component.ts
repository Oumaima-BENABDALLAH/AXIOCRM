import { Component } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ProductApp';
  showSidebar = true;
  constructor(private router: Router) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe((event: NavigationEnd) => {
      const urlPath = event.urlAfterRedirects.split('?')[0];
      console.log("Route actuelle :", event.urlAfterRedirects);
      /*this.showSidebar = event.urlAfterRedirects !== '/login' && event.urlAfterRedirects !== '/signup' && event.urlAfterRedirects !=='/reset-password' && event.urlAfterRedirects !=='/forgot-password';*/
     this.showSidebar = urlPath !== '/login' && urlPath !== '/signup' && urlPath !== '/reset-password' && urlPath !== '/forgot-password';

    });
  }
}
