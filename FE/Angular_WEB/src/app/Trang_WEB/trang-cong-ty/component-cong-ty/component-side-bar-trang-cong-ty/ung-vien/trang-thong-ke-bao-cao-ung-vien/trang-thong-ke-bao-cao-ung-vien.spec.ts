import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongKeBaoCaoUngVien } from './trang-thong-ke-bao-cao-ung-vien';

describe('TrangThongKeBaoCaoUngVien', () => {
  let component: TrangThongKeBaoCaoUngVien;
  let fixture: ComponentFixture<TrangThongKeBaoCaoUngVien>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongKeBaoCaoUngVien]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongKeBaoCaoUngVien);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
