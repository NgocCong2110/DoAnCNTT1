import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheChuyenTrangTaoCv } from './the-chuyen-trang-tao-cv';

describe('TheChuyenTrangTaoCv', () => {
  let component: TheChuyenTrangTaoCv;
  let fixture: ComponentFixture<TheChuyenTrangTaoCv>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TheChuyenTrangTaoCv]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TheChuyenTrangTaoCv);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
