import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachThongBao } from './trang-danh-sach-thong-bao';

describe('TrangDanhSachThongBao', () => {
  let component: TrangDanhSachThongBao;
  let fixture: ComponentFixture<TrangDanhSachThongBao>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachThongBao]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachThongBao);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
