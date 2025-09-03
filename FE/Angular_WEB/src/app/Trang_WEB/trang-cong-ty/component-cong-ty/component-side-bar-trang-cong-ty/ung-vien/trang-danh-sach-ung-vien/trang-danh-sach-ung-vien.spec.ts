import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachUngVien } from './trang-danh-sach-ung-vien';

describe('TrangDanhSachUngVien', () => {
  let component: TrangDanhSachUngVien;
  let fixture: ComponentFixture<TrangDanhSachUngVien>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachUngVien]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachUngVien);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
