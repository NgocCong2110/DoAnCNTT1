import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachCongTy } from './trang-danh-sach-cong-ty';

describe('TrangDanhSachCongTy', () => {
  let component: TrangDanhSachCongTy;
  let fixture: ComponentFixture<TrangDanhSachCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
