import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachGoiDichVuCongTy } from './trang-danh-sach-goi-dich-vu-cong-ty';

describe('TrangDanhSachGoiDichVuCongTy', () => {
  let component: TrangDanhSachGoiDichVuCongTy;
  let fixture: ComponentFixture<TrangDanhSachGoiDichVuCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachGoiDichVuCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachGoiDichVuCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
