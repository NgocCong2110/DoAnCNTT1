import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongTinTaiKhoanNtv } from './trang-thong-tin-tai-khoan-ntv';

describe('TrangThongTinTaiKhoanNtv', () => {
  let component: TrangThongTinTaiKhoanNtv;
  let fixture: ComponentFixture<TrangThongTinTaiKhoanNtv>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongTinTaiKhoanNtv]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongTinTaiKhoanNtv);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
