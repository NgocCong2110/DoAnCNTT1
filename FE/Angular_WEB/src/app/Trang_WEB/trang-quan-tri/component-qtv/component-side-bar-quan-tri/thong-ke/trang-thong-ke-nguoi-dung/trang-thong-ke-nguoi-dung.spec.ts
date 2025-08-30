import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongKeNguoiDung } from './trang-thong-ke-nguoi-dung';

describe('TrangThongKeNguoiDung', () => {
  let component: TrangThongKeNguoiDung;
  let fixture: ComponentFixture<TrangThongKeNguoiDung>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongKeNguoiDung]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongKeNguoiDung);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
