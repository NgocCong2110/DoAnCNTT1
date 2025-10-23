import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheDanhGiaTuNguoiDung } from './the-danh-gia-tu-nguoi-dung';

describe('TheDanhGiaTuNguoiDung', () => {
  let component: TheDanhGiaTuNguoiDung;
  let fixture: ComponentFixture<TheDanhGiaTuNguoiDung>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TheDanhGiaTuNguoiDung]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TheDanhGiaTuNguoiDung);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
