import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachViPham } from './trang-danh-sach-vi-pham';

describe('TrangDanhSachViPham', () => {
  let component: TrangDanhSachViPham;
  let fixture: ComponentFixture<TrangDanhSachViPham>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachViPham]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachViPham);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
