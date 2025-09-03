import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongTinTaiKhoanCongTy } from './trang-thong-tin-tai-khoan-cong-ty';

describe('TrangThongTinTaiKhoanCongTy', () => {
  let component: TrangThongTinTaiKhoanCongTy;
  let fixture: ComponentFixture<TrangThongTinTaiKhoanCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongTinTaiKhoanCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongTinTaiKhoanCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
