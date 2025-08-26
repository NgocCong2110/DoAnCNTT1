import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangNguoiTimViec } from './trang-nguoi-tim-viec';

describe('TrangNguoiTimViec', () => {
  let component: TrangNguoiTimViec;
  let fixture: ComponentFixture<TrangNguoiTimViec>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangNguoiTimViec]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangNguoiTimViec);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
