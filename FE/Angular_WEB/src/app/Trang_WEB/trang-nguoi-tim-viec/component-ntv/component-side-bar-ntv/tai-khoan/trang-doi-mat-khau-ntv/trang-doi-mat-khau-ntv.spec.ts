import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDoiMatKhauNtv } from './trang-doi-mat-khau-ntv';

describe('TrangDoiMatKhauNtv', () => {
  let component: TrangDoiMatKhauNtv;
  let fixture: ComponentFixture<TrangDoiMatKhauNtv>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDoiMatKhauNtv]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDoiMatKhauNtv);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
