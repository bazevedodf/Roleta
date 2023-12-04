import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroListaComponent } from './cadastro-lista.component';

describe('CadastroListaComponent', () => {
  let component: CadastroListaComponent;
  let fixture: ComponentFixture<CadastroListaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CadastroListaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CadastroListaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
