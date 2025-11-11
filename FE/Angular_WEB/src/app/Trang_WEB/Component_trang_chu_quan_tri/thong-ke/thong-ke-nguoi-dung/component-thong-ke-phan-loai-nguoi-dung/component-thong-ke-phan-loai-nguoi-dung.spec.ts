import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentThongKePhanLoaiNguoiDung } from './component-thong-ke-phan-loai-nguoi-dung';

describe('ComponentThongKePhanLoaiNguoiDung', () => {
  let component: ComponentThongKePhanLoaiNguoiDung;
  let fixture: ComponentFixture<ComponentThongKePhanLoaiNguoiDung>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentThongKePhanLoaiNguoiDung]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentThongKePhanLoaiNguoiDung);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
