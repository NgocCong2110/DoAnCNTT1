import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangCvCuaNguoiTimViec } from './trang-cv-cua-nguoi-tim-viec';

describe('TrangCvCuaNguoiTimViec', () => {
  let component: TrangCvCuaNguoiTimViec;
  let fixture: ComponentFixture<TrangCvCuaNguoiTimViec>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangCvCuaNguoiTimViec]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangCvCuaNguoiTimViec);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
