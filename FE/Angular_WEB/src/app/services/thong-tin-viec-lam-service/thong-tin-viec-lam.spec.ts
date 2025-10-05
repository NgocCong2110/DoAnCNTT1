import { TestBed } from '@angular/core/testing';

import { ThongTinViecLam } from './thong-tin-viec-lam';

describe('ThongTinViecLam', () => {
  let service: ThongTinViecLam;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ThongTinViecLam);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
