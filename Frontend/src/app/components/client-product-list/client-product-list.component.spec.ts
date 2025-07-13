import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientProductListComponent } from './client-product-list.component';

describe('ClientProductListComponent', () => {
  let component: ClientProductListComponent;
  let fixture: ComponentFixture<ClientProductListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientProductListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientProductListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
