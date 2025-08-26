import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBarTrangQuanTri } from './side-bar-trang-quan-tri';

describe('SideBarTrangQuanTri', () => {
  let component: SideBarTrangQuanTri;
  let fixture: ComponentFixture<SideBarTrangQuanTri>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideBarTrangQuanTri]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideBarTrangQuanTri);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
