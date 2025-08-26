import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBarTrangCongTy } from './side-bar-trang-cong-ty';

describe('SideBarTrangCongTy', () => {
  let component: SideBarTrangCongTy;
  let fixture: ComponentFixture<SideBarTrangCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideBarTrangCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideBarTrangCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
