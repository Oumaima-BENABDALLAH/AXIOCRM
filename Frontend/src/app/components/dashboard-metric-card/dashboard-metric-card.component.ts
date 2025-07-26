import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-dashboard-metric-card',
  templateUrl: './dashboard-metric-card.component.html',
  styleUrls: ['./dashboard-metric-card.component.css']
})
export class DashboardMetricCardComponent {
  @Input() label!: string;
  @Input() value!: string;
}
