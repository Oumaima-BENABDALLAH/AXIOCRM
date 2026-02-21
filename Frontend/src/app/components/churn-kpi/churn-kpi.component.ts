import { Component, Input } from '@angular/core';
import { ChurnDashboard } from '../../models/churn-dashboard.model';

@Component({
  selector: 'app-churn-kpi',
  templateUrl: './churn-kpi.component.html',
  styleUrls: ['./churn-kpi.component.css']
})
export class ChurnKpiComponent {
  @Input() churn!: ChurnDashboard;

  get riskColor(): string {
    if (!this.churn) return '#ccc';
    if (this.churn.riskRate > 70) return '#ef4444'; 
    if (this.churn.riskRate > 40) return '#f97316'; 
    return '#22c55e'; 
  }
}