import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentThongKeTinTuyenDung } from './component-thong-ke-tin-tuyen-dung';

describe('ComponentThongKeTinTuyenDung', () => {
  let component: ComponentThongKeTinTuyenDung;
  let fixture: ComponentFixture<ComponentThongKeTinTuyenDung>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentThongKeTinTuyenDung]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentThongKeTinTuyenDung);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
