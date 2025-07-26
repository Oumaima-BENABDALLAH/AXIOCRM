import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardMetricCardComponent } from './dashboard-metric-card.component';

describe('DashboardMetricCardComponent', () => {
  let component: DashboardMetricCardComponent;
  let fixture: ComponentFixture<DashboardMetricCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardMetricCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DashboardMetricCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
