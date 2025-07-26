import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TotalProjectsCardComponent } from './total-projects-card.component';

describe('TotalProjectsCardComponent', () => {
  let component: TotalProjectsCardComponent;
  let fixture: ComponentFixture<TotalProjectsCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TotalProjectsCardComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TotalProjectsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
