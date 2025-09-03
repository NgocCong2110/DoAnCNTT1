import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangBaiDangDaXoaCongTy } from './trang-bai-dang-da-xoa-cong-ty';

describe('TrangBaiDangDaXoaCongTy', () => {
  let component: TrangBaiDangDaXoaCongTy;
  let fixture: ComponentFixture<TrangBaiDangDaXoaCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangBaiDangDaXoaCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangBaiDangDaXoaCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
