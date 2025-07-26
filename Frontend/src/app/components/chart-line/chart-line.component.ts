import { Component } from '@angular/core';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexTitleSubtitle,
} from 'ng-apexcharts';

@Component({
  selector: 'app-chart-line',
  templateUrl: './chart-line.component.html',
  styleUrls: ['./chart-line.component.css']
})
export class ChartLineComponent {
  chartOptions = {
    series: [
      {
        name: '2020 Report',
        data: [10, 41, 35, 51, 49, 62, 69]
      },
      {
        name: '2021 Report',
        data: [20, 30, 40, 50, 60, 70, 80]
      }
    ],
    chart: {
      height: 300,
      type: 'line'
    },
    title: {
      text: 'Analytics Report'
    },
    xaxis: {
      categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul']
    }
  };
}
