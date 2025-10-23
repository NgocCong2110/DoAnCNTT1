import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MauCvMacDinh } from './mau-cv-mac-dinh';

describe('MauCvMacDinh', () => {
  let component: MauCvMacDinh;
  let fixture: ComponentFixture<MauCvMacDinh>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MauCvMacDinh]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MauCvMacDinh);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
