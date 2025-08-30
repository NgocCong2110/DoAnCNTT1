import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangTaoTaiKhoanQuanTri } from './trang-tao-tai-khoan-quan-tri';

describe('TrangTaoTaiKhoanQuanTri', () => {
  let component: TrangTaoTaiKhoanQuanTri;
  let fixture: ComponentFixture<TrangTaoTaiKhoanQuanTri>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangTaoTaiKhoanQuanTri]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangTaoTaiKhoanQuanTri);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
