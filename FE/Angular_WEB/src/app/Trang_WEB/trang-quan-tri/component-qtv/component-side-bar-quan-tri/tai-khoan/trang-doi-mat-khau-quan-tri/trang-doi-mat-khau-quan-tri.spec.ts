import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDoiMatKhauQuanTri } from './trang-doi-mat-khau-quan-tri';

describe('TrangDoiMatKhauQuanTri', () => {
  let component: TrangDoiMatKhauQuanTri;
  let fixture: ComponentFixture<TrangDoiMatKhauQuanTri>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDoiMatKhauQuanTri]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDoiMatKhauQuanTri);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
