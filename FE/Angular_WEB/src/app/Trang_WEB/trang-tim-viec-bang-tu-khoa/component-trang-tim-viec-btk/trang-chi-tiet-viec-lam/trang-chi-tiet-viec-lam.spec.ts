import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangChiTietViecLam } from './trang-chi-tiet-viec-lam';

describe('TrangChiTietViecLam', () => {
  let component: TrangChiTietViecLam;
  let fixture: ComponentFixture<TrangChiTietViecLam>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangChiTietViecLam]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangChiTietViecLam);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
