import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangQuenMatKhau } from './trang-quen-mat-khau';

describe('TrangQuenMatKhau', () => {
  let component: TrangQuenMatKhau;
  let fixture: ComponentFixture<TrangQuenMatKhau>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangQuenMatKhau]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangQuenMatKhau);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
