import { Component } from '@angular/core';
import { ChartConfiguration, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'app-charts-section',
  templateUrl: './charts-section.component.html',
  styleUrls: ['./charts-section.component.css']
})
export class ChartsSectionComponent {
  
  barChartData = {
    labels: ['2020', '2021', '2022', '2023', '2024', '2025'],
    datasets: [
      {
        label: 'Total',
        data: [65, 59, 80, 81, 56, 55],
        backgroundColor: '#FFB6C1',
      },
      {
        label: 'To do',
        data: [28, 48, 40, 19, 86, 27],
        backgroundColor: '#FFA07A',
      },
      {
        label: 'Completed',
        data: [35, 25, 60, 55, 45, 80],
        backgroundColor: '#9370DB',
      },
      {
        label: 'Overdue',
        data: [12, 20, 10, 30, 15, 18],
        backgroundColor: '#8A2BE2',
      }
    ]
  };

  lineChartData = {
    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
    datasets: [
      {
        label: '2022 Report',
        data: [30, 50, 40, 60, 35, 70, 50],
        borderColor: '#FF6384',
        backgroundColor: 'transparent',
        tension: 0.4,
        pointRadius: 4,
      },
      {
        label: '2024 Report',
        data: [50, 60, 55, 40, 65, 60, 75],
        borderColor: '#36A2EB',
        backgroundColor: 'transparent',
        tension: 0.4,
        pointRadius: 4,
      }
    ]
  };
}
