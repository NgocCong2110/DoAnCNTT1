import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheViecLamDuocQuanTam } from './the-viec-lam-duoc-quan-tam';

describe('TheViecLamDuocQuanTam', () => {
  let component: TheViecLamDuocQuanTam;
  let fixture: ComponentFixture<TheViecLamDuocQuanTam>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TheViecLamDuocQuanTam]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TheViecLamDuocQuanTam);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
