import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoletaComponent } from './roleta.component';

describe('RoletaComponent', () => {
  let component: RoletaComponent;
  let fixture: ComponentFixture<RoletaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RoletaComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoletaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
