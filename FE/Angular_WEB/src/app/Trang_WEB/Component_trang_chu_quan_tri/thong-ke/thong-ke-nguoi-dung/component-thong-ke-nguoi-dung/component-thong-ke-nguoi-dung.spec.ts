import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentThongKeNguoiDung } from './component-thong-ke-nguoi-dung';

describe('ComponentThongKeNguoiDung', () => {
  let component: ComponentThongKeNguoiDung;
  let fixture: ComponentFixture<ComponentThongKeNguoiDung>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentThongKeNguoiDung]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentThongKeNguoiDung);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
