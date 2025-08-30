import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachNguoiTimViec } from './trang-danh-sach-nguoi-tim-viec';

describe('TrangDanhSachNguoiTimViec', () => {
  let component: TrangDanhSachNguoiTimViec;
  let fixture: ComponentFixture<TrangDanhSachNguoiTimViec>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachNguoiTimViec]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachNguoiTimViec);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
