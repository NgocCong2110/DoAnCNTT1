import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachGoiDichVuQuanTri } from './trang-danh-sach-goi-dich-vu-quan-tri';

describe('TrangDanhSachGoiDichVuQuanTri', () => {
  let component: TrangDanhSachGoiDichVuQuanTri;
  let fixture: ComponentFixture<TrangDanhSachGoiDichVuQuanTri>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachGoiDichVuQuanTri]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachGoiDichVuQuanTri);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
