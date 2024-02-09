import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AfiliadoListaComponent } from './afiliado-lista.component';

describe('AfiliadoListaComponent', () => {
  let component: AfiliadoListaComponent;
  let fixture: ComponentFixture<AfiliadoListaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AfiliadoListaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AfiliadoListaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
