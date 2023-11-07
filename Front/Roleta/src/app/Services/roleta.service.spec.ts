import { TestBed } from '@angular/core/testing';

import { RoletaService } from './roleta.service';

describe('RoletaService', () => {
  let service: RoletaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoletaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
