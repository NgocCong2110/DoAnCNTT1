import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangLichSuUngTuyen } from './trang-lich-su-ung-tuyen';

describe('TrangLichSuUngTuyen', () => {
  let component: TrangLichSuUngTuyen;
  let fixture: ComponentFixture<TrangLichSuUngTuyen>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangLichSuUngTuyen]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangLichSuUngTuyen);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
