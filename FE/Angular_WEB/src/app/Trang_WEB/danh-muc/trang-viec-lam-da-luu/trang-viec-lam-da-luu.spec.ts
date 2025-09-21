import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangViecLamDaLuu } from './trang-viec-lam-da-luu';

describe('TrangViecLamDaLuu', () => {
  let component: TrangViecLamDaLuu;
  let fixture: ComponentFixture<TrangViecLamDaLuu>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangViecLamDaLuu]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangViecLamDaLuu);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
