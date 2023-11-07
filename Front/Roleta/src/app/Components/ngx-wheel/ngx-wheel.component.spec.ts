import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NgxWheelComponent } from './ngx-wheel.component';

describe('NgxWheelComponent', () => {
  let component: NgxWheelComponent;
  let fixture: ComponentFixture<NgxWheelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NgxWheelComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NgxWheelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
