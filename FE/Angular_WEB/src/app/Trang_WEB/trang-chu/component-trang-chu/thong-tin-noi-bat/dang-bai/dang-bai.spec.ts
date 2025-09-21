import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DangBai } from './dang-bai';

describe('DangBai', () => {
  let component: DangBai;
  let fixture: ComponentFixture<DangBai>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DangBai]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DangBai);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
