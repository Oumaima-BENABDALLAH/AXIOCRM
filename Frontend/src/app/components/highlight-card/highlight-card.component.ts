import { Component,Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgApexchartsModule } from 'ng-apexcharts';
import {
  ApexChart,
  ApexStroke,
  ApexTooltip,
  ApexMarkers,
  ApexDataLabels,
  ApexXAxis
} from 'ng-apexcharts';

@Component({
  selector: 'app-highlight-card',
  standalone: true,
  imports: [CommonModule, NgApexchartsModule],
  templateUrl: './highlight-card.component.html',
  styleUrl: './highlight-card.component.css'
})
export class HighlightCardComponent implements OnInit{
   @Input() value: string = '3.4K';
   @Input() label: string = 'This Month';
   @Input() data: number[] = [100, 150, 130, 170, 160, 200, 267];
  public chart: ApexChart = {
    type: 'line',
    height: 60,
    sparkline: { enabled: true }
  };

  public stroke: ApexStroke = {
    curve: 'smooth',
    width: 2
  };

  public tooltip: ApexTooltip = {
    enabled: false
  };

public markers: ApexMarkers = {
  size: 0,
  colors: ['#ffffff'],
  strokeWidth: 0,
  discrete: [
    {
      seriesIndex: 0,
      dataPointIndex: this.data.length - 1,
      fillColor: '#ffffff',
      strokeColor: '#ffffff',
      size: 5
    }
  ]
};
public dataLabels!: ApexDataLabels;
  public xaxis: ApexXAxis = {
  labels: {
    show: false
  },
  axisBorder: {
    show: false
  },
  axisTicks: {
    show: false
  }
};

ngOnInit(): void {
  const lastIndex = this.data.length - 1;

  this.markers = {
    size: 0,
    strokeWidth: 0,
    discrete: [
      {
        seriesIndex: 0,
        dataPointIndex: lastIndex,
        fillColor: '#ffffff',
        strokeColor: '#ffffff',
        size: 5
      }
    ]
  };

  this.dataLabels = {
    enabled: true,
    enabledOnSeries: [0],
    style: {
      fontSize: '12px',
      colors: ['#ffffff']
    },
    offsetY: -4,
    offsetX:-4,
    formatter: (val: any, opts: any) => {
      const isLast = opts.dataPointIndex === lastIndex;
      return isLast ? val.toString() : undefined;
    },
    background: {
    enabled: true,
    foreColor: '#ffffff',
    padding: 4,
    borderRadius: 4,
    borderWidth: 0,
    opacity: 0, 
    dropShadow: {
      enabled: false
    }
  },
    dropShadow: {
      enabled: false
    }
  };
}

}
