import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongKeDoanhThu } from './trang-thong-ke-doanh-thu';

describe('TrangThongKeDoanhThu', () => {
  let component: TrangThongKeDoanhThu;
  let fixture: ComponentFixture<TrangThongKeDoanhThu>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongKeDoanhThu]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongKeDoanhThu);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
