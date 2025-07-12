import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order.service';


@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.css']
})
export class DashboardHomeComponent implements OnInit {
  totalEarnings: number = 0;
  totalBalance: number = 0;
  totalProjects: number = 0;
  constructor(private orderService : OrderService) { }

  ngOnInit() {
    this.loadDashboardData();
  }
  loadDashboardData(): void {
    this.orderService.getDashboardStats().subscribe(data => {
      this.totalEarnings = data.totalEarnings;
      this.totalBalance = data.totalBalance;
      this.totalProjects = data.totalProjects;
    });
  }
}
