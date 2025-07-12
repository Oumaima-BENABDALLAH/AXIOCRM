import { Component, AfterViewInit  } from '@angular/core';
import {
  ApexNonAxisChartSeries,
  ApexChart,
  ApexAxisChartSeries,
  ApexStroke,
  ApexDataLabels,
  ApexTooltip,
  ApexXAxis,
  ChartComponent
} from "ng-apexcharts";


export type ChartOptions = {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  stroke: ApexStroke;
  tooltip: ApexTooltip;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
};
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  public chartLineSparkline1Options: Partial<ChartOptions>;
  public commonLineSparklineOptions: Partial<ChartOptions>;

  constructor() {
    this.chartLineSparkline1Options = {
      series: [
        {
          name: "Earnings",
          data: [5, 10, 7, 12, 8, 14, 10]
        }
      ]
    };

    this.commonLineSparklineOptions = {
      chart: {
        type: "line",
        height: 40,
        sparkline: {
          enabled: true // <-- c'est ça qui supprime axes + bordures
        }
      },
      stroke: {
        curve: "smooth",
        width: 2
      },
      tooltip: {
        enabled: false
      },
      dataLabels: {
        enabled: false
      },
      xaxis: {
        labels: { show: false },
        axisBorder: { show: false },
        axisTicks: { show: false }
      }
    };
  }
}