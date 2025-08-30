import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangTimViecXungQuanh } from './trang-tim-viec-xung-quanh';

describe('TrangTimViecXungQuanh', () => {
  let component: TrangTimViecXungQuanh;
  let fixture: ComponentFixture<TrangTimViecXungQuanh>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangTimViecXungQuanh]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangTimViecXungQuanh);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
