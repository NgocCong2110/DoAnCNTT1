import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThemGoiTin } from './trang-them-goi-tin';

describe('TrangThemGoiTin', () => {
  let component: TrangThemGoiTin;
  let fixture: ComponentFixture<TrangThemGoiTin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThemGoiTin]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThemGoiTin);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
