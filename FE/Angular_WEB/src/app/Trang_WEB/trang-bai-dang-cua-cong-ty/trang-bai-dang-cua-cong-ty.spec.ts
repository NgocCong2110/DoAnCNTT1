import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangBaiDangCuaCongTy } from './trang-bai-dang-cua-cong-ty';

describe('TrangBaiDangCuaCongTy', () => {
  let component: TrangBaiDangCuaCongTy;
  let fixture: ComponentFixture<TrangBaiDangCuaCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangBaiDangCuaCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangBaiDangCuaCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
