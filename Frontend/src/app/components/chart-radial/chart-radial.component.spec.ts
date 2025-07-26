import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartRadialComponent } from './chart-radial.component';

describe('ChartRadialComponent', () => {
  let component: ChartRadialComponent;
  let fixture: ComponentFixture<ChartRadialComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChartRadialComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ChartRadialComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
