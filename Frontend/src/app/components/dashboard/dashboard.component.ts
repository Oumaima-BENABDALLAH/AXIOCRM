import { Component ,OnInit  } from '@angular/core';
import { SparklineCardComponent } from '../sparkline-card/sparkline-card.component';
import { NotificationService } from '../../services/notification.service';
import { ChurnDashboard } from '../../models/churn-dashboard.model';
import { ChurnService } from '../../services/churn.service';
import { AuthService } from '../../services/auth.service';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexStroke,
  ApexTooltip
   

} from 'ng-apexcharts';
import { Router } from '@angular/router';
export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  stroke: ApexStroke;
  colors: string[];
  tooltip: ApexTooltip;
};
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent  implements OnInit {
  churn!: ChurnDashboard;
  public chartOptions: ChartOptions;
  franceTime: string = '';
  notifications: string[] = [];
  showNotifications: boolean = false;
  showLogoutText = false;
  profilePictureUrl: string = '';

  constructor(private churnService: ChurnService,private notificationService: NotificationService,private router :Router,private authService :AuthService) {
    this.chartOptions = {
      series: [
        {
          name: 'Earnings',
          data: [1.2, 1.1, 1.3, 1.25, 1.15, 1.05, 1.2] 
        }
      ],
      chart: {
        type: 'line',
        height: 50,
        width: 100,
        sparkline: {
          enabled: true
        }
      },
      stroke: {
        curve: 'smooth',
        width: 2
      },
      colors: ['#3f51b5'], 
      tooltip: {
        enabled: false
      }
    };
  }
 
 ngOnInit(): void {
    this.loadChurn();
    this.updateTimes();
    setInterval(() => this.updateTimes(), 1000); 
    this.notificationService.notification$
    .subscribe(n => {
      if (!n) return;

      console.log('EVENT:', n.title, n.message);
    });

    this.profilePictureUrl = this.authService.getProfilePicture();
  } 

 updateTimes() {
    const options: Intl.DateTimeFormatOptions = {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit',
      day: 'numeric',
      month: 'long',
      year: 'numeric',
      hour12: false
    };

    this.franceTime = new Date().toLocaleString('fr-FR', {
      ...options,
      timeZone: 'Europe/Paris'
    });
  }
  toggleNotifications() {
  this.showNotifications = !this.showNotifications;
}
 toggleLogout() {
    this.showLogoutText = !this.showLogoutText;
     if (this.showLogoutText){
      setTimeout(()=>{
        this.toggleLogout();
      },1000)
     }
     
  localStorage.removeItem('token');
   this.authService.logout();

} 
loadChurn() {
    this.churnService.getDashboard()
      .subscribe(res => this.churn = res);
  }
}