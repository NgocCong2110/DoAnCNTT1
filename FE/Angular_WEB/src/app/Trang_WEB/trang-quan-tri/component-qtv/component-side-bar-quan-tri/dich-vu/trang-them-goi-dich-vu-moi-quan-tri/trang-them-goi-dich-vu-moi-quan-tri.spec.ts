import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThemGoiDichVuMoiQuanTri } from './trang-them-goi-dich-vu-moi-quan-tri';

describe('TrangThemGoiDichVuMoiQuanTri', () => {
  let component: TrangThemGoiDichVuMoiQuanTri;
  let fixture: ComponentFixture<TrangThemGoiDichVuMoiQuanTri>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThemGoiDichVuMoiQuanTri]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThemGoiDichVuMoiQuanTri);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
