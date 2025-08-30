import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongTinTaiKhoan } from './trang-thong-tin-tai-khoan';

describe('TrangThongTinTaiKhoan', () => {
  let component: TrangThongTinTaiKhoan;
  let fixture: ComponentFixture<TrangThongTinTaiKhoan>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongTinTaiKhoan]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongTinTaiKhoan);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
