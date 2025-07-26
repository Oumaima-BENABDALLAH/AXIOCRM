import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SparklineCardComponent } from './sparkline-card.component';

describe('SparklineCardComponent', () => {
  let component: SparklineCardComponent;
  let fixture: ComponentFixture<SparklineCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SparklineCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SparklineCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
