import { Component, AfterViewInit } from '@angular/core';
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
export class DashboardComponent {
  public chartOptions: ChartOptions;

  constructor() {
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
}