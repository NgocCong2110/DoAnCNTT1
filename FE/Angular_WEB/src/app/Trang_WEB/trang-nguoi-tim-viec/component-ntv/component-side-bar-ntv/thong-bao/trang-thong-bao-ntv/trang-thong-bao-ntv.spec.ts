import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongBaoNtv } from './trang-thong-bao-ntv';

describe('TrangThongBaoNtv', () => {
  let component: TrangThongBaoNtv;
  let fixture: ComponentFixture<TrangThongBaoNtv>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongBaoNtv]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongBaoNtv);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
