import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangGioiThieuCuaCongTy } from './trang-gioi-thieu-cua-cong-ty';

describe('TrangGioiThieuCuaCongTy', () => {
  let component: TrangGioiThieuCuaCongTy;
  let fixture: ComponentFixture<TrangGioiThieuCuaCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangGioiThieuCuaCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangGioiThieuCuaCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
