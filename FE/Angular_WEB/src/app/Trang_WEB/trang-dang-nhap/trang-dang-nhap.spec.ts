import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDangNhap } from './trang-dang-nhap';

describe('TrangDangNhap', () => {
  let component: TrangDangNhap;
  let fixture: ComponentFixture<TrangDangNhap>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDangNhap]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDangNhap);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
