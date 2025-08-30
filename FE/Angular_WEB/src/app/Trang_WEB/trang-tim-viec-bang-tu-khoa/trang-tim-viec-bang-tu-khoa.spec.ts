import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangTimViecBangTuKhoa } from './trang-tim-viec-bang-tu-khoa';

describe('TrangTimViecBangTuKhoa', () => {
  let component: TrangTimViecBangTuKhoa;
  let fixture: ComponentFixture<TrangTimViecBangTuKhoa>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangTimViecBangTuKhoa]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangTimViecBangTuKhoa);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
