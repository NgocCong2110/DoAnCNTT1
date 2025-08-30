import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheTimKiemViecLam } from './the-tim-kiem-viec-lam';

describe('TheTimKiemViecLam', () => {
  let component: TheTimKiemViecLam;
  let fixture: ComponentFixture<TheTimKiemViecLam>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TheTimKiemViecLam]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TheTimKiemViecLam);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
