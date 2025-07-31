import { Component ,OnInit  } from '@angular/core';
import { SparklineCardComponent } from '../sparkline-card/sparkline-card.component';
import { NotificationService } from '../../services/notification.service';

import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexStroke,
  ApexTooltip
   

} from 'ng-apexcharts';
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
  public chartOptions: ChartOptions;
  franceTime: string = '';
  notifications: string[] = [];
  showNotifications: boolean = false;
  constructor(private notificationService: NotificationService) {
    this.chartOptions = {
      series: [
        {
          name: 'Earnings',
          data: [1.2, 1.1, 1.3, 1.25, 1.15, 1.05, 1.2] // Exemple de valeurs
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
      colors: ['#3f51b5'], // Bleu violet comme sur ton image
      tooltip: {
        enabled: false
      }
    };
  }
 
 ngOnInit(): void {
    this.updateTimes();
    setInterval(() => this.updateTimes(), 1000); // mise à jour chaque seconde
    this.notificationService.notification$.subscribe((message) => {
      if (message) {
        this.notifications.push(message);
      }
    });
  
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
}