import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MauCvMacDinh2 } from './mau-cv-mac-dinh-2';

describe('MauCvMacDinh2', () => {
  let component: MauCvMacDinh2;
  let fixture: ComponentFixture<MauCvMacDinh2>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MauCvMacDinh2]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MauCvMacDinh2);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
