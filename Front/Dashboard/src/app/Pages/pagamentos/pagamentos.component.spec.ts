import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PagamentosComponent } from './pagamentos.component';

describe('PagamentosComponent', () => {
  let component: PagamentosComponent;
  let fixture: ComponentFixture<PagamentosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PagamentosComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PagamentosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
