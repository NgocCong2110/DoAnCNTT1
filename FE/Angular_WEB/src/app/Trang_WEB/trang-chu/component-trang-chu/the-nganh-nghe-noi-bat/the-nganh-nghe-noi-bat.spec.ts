import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheNganhNgheNoiBat } from './the-nganh-nghe-noi-bat';

describe('TheNganhNgheNoiBat', () => {
  let component: TheNganhNgheNoiBat;
  let fixture: ComponentFixture<TheNganhNgheNoiBat>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TheNganhNgheNoiBat]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TheNganhNgheNoiBat);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
