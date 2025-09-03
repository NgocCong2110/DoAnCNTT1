import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachBaiDangCongTy } from './trang-danh-sach-bai-dang-cong-ty';

describe('TrangDanhSachBaiDangCongTy', () => {
  let component: TrangDanhSachBaiDangCongTy;
  let fixture: ComponentFixture<TrangDanhSachBaiDangCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachBaiDangCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachBaiDangCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
