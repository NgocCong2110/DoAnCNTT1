import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangLuaChonDangKy } from './trang-lua-chon-dang-ky';

describe('TrangLuaChonDangKy', () => {
  let component: TrangLuaChonDangKy;
  let fixture: ComponentFixture<TrangLuaChonDangKy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangLuaChonDangKy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangLuaChonDangKy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
