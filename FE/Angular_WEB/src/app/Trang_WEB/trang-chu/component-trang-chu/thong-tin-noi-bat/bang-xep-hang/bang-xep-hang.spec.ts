import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BangXepHang } from './bang-xep-hang';

describe('BangXepHang', () => {
  let component: BangXepHang;
  let fixture: ComponentFixture<BangXepHang>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BangXepHang]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BangXepHang);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
