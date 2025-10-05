import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDoiMatKhau } from './trang-doi-mat-khau';

describe('TrangDoiMatKhau', () => {
  let component: TrangDoiMatKhau;
  let fixture: ComponentFixture<TrangDoiMatKhau>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDoiMatKhau]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDoiMatKhau);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
