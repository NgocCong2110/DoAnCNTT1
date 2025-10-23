import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MauCvCongNghe } from './mau-cv-cong-nghe';

describe('MauCvCongNghe', () => {
  let component: MauCvCongNghe;
  let fixture: ComponentFixture<MauCvCongNghe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MauCvCongNghe]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MauCvCongNghe);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
