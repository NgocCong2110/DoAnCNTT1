import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentBaoCaoViPham } from './component-bao-cao-vi-pham';

describe('ComponentBaoCaoViPham', () => {
  let component: ComponentBaoCaoViPham;
  let fixture: ComponentFixture<ComponentBaoCaoViPham>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentBaoCaoViPham]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentBaoCaoViPham);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
