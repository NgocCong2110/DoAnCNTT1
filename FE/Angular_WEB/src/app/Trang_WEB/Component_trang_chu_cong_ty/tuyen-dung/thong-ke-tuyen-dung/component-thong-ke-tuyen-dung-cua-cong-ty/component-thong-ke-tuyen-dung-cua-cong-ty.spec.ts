import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentThongKeTuyenDungCuaCongTy } from './component-thong-ke-tuyen-dung-cua-cong-ty';

describe('ComponentThongKeTuyenDungCuaCongTy', () => {
  let component: ComponentThongKeTuyenDungCuaCongTy;
  let fixture: ComponentFixture<ComponentThongKeTuyenDungCuaCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentThongKeTuyenDungCuaCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentThongKeTuyenDungCuaCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
