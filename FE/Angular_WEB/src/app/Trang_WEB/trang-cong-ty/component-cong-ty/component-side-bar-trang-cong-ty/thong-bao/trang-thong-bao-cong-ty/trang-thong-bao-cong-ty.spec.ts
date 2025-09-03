import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongBaoCongTy } from './trang-thong-bao-cong-ty';

describe('TrangThongBaoCongTy', () => {
  let component: TrangThongBaoCongTy;
  let fixture: ComponentFixture<TrangThongBaoCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongBaoCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongBaoCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
