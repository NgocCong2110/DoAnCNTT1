import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDoiMatKhauCongTy } from './trang-doi-mat-khau-cong-ty';

describe('TrangDoiMatKhauCongTy', () => {
  let component: TrangDoiMatKhauCongTy;
  let fixture: ComponentFixture<TrangDoiMatKhauCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDoiMatKhauCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDoiMatKhauCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
