import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDuyetDanhGiaWeb } from './trang-duyet-danh-gia-web';

describe('TrangDuyetDanhGiaWeb', () => {
  let component: TrangDuyetDanhGiaWeb;
  let fixture: ComponentFixture<TrangDuyetDanhGiaWeb>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDuyetDanhGiaWeb]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDuyetDanhGiaWeb);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
