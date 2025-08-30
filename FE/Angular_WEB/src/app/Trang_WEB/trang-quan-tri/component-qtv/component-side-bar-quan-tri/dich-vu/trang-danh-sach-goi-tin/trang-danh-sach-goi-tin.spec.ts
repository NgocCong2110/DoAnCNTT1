import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachGoiTin } from './trang-danh-sach-goi-tin';

describe('TrangDanhSachGoiTin', () => {
  let component: TrangDanhSachGoiTin;
  let fixture: ComponentFixture<TrangDanhSachGoiTin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachGoiTin]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachGoiTin);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
