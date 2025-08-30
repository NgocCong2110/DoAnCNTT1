import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhMucKyNang } from './trang-danh-muc-ky-nang';

describe('TrangDanhMucKyNang', () => {
  let component: TrangDanhMucKyNang;
  let fixture: ComponentFixture<TrangDanhMucKyNang>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhMucKyNang]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhMucKyNang);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
