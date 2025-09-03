import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhSachChungChi } from './trang-danh-sach-chung-chi';

describe('TrangDanhSachChungChi', () => {
  let component: TrangDanhSachChungChi;
  let fixture: ComponentFixture<TrangDanhSachChungChi>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhSachChungChi]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhSachChungChi);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
