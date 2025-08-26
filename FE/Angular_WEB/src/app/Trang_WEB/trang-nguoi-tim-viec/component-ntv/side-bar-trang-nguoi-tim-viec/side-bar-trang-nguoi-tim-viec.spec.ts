import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBarTrangNguoiTimViec } from './side-bar-trang-nguoi-tim-viec';

describe('SideBarTrangNguoiTimViec', () => {
  let component: SideBarTrangNguoiTimViec;
  let fixture: ComponentFixture<SideBarTrangNguoiTimViec>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideBarTrangNguoiTimViec]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideBarTrangNguoiTimViec);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
