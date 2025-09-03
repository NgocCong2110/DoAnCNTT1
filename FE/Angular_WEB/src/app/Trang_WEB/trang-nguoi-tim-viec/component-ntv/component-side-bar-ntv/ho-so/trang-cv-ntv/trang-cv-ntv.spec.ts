import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangCvNtv } from './trang-cv-ntv';

describe('TrangCvNtv', () => {
  let component: TrangCvNtv;
  let fixture: ComponentFixture<TrangCvNtv>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangCvNtv]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangCvNtv);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
