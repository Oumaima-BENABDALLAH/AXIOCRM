import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AvailableBalanceCardComponent } from './available-balance-card.component';

describe('AvailableBalanceCardComponent', () => {
  let component: AvailableBalanceCardComponent;
  let fixture: ComponentFixture<AvailableBalanceCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AvailableBalanceCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AvailableBalanceCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
