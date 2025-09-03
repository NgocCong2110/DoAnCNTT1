import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongTinCaNhanNtv } from './trang-thong-tin-ca-nhan-ntv';

describe('TrangThongTinCaNhanNtv', () => {
  let component: TrangThongTinCaNhanNtv;
  let fixture: ComponentFixture<TrangThongTinCaNhanNtv>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongTinCaNhanNtv]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongTinCaNhanNtv);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
