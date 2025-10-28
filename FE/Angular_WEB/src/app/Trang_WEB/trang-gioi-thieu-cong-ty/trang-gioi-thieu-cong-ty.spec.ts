import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangGioiThieuCongTy } from './trang-gioi-thieu-cong-ty';

describe('TrangGioiThieuCongTy', () => {
  let component: TrangGioiThieuCongTy;
  let fixture: ComponentFixture<TrangGioiThieuCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangGioiThieuCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangGioiThieuCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
