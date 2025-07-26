import { Component } from '@angular/core';
import {
  ApexNonAxisChartSeries,
  ApexChart,
  ApexFill,
  ApexStroke,
} from 'ng-apexcharts';

@Component({
  selector: 'app-chart-radial',
  templateUrl: './chart-radial.component.html',
  styleUrls: ['./chart-radial.component.css']
})
export class ChartRadialComponent {
  chartOptions = {
    series: [85],
    chart: {
      height: 300,
      type: 'radialBar'
    },
    labels: ['This Month'],
    fill: {
      colors: ['#4F46E5']
    },
    stroke: {
      lineCap: 'round'
    }
  };
}
