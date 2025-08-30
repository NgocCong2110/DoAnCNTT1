import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangLichSuGiaoDich } from './trang-lich-su-giao-dich';

describe('TrangLichSuGiaoDich', () => {
  let component: TrangLichSuGiaoDich;
  let fixture: ComponentFixture<TrangLichSuGiaoDich>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangLichSuGiaoDich]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangLichSuGiaoDich);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
