import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDangKyNTV } from './trang-dang-ky-ntv';

describe('TrangDangKyNTV', () => {
  let component: TrangDangKyNTV;
  let fixture: ComponentFixture<TrangDangKyNTV>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDangKyNTV]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDangKyNTV);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
