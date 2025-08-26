import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDangKyCongTy } from './trang-dang-ky-cong-ty';

describe('TrangDangKyCongTy', () => {
  let component: TrangDangKyCongTy;
  let fixture: ComponentFixture<TrangDangKyCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDangKyCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDangKyCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
