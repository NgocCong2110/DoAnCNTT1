import { TestBed } from '@angular/core/testing';

import { BaiDang } from './bai-dang';

describe('BaiDang', () => {
  let service: BaiDang;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BaiDang);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
