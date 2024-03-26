/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RoletaService } from './roleta.service';

describe('Service: Roleta', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RoletaService]
    });
  });

  it('should ...', inject([RoletaService], (service: RoletaService) => {
    expect(service).toBeTruthy();
  }));
});
