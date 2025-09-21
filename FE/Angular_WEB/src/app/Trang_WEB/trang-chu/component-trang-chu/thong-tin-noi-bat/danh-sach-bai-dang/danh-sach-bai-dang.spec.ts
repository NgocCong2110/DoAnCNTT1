import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DanhSachBaiDang } from './danh-sach-bai-dang';

describe('DanhSachBaiDang', () => {
  let component: DanhSachBaiDang;
  let fixture: ComponentFixture<DanhSachBaiDang>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DanhSachBaiDang]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DanhSachBaiDang);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
