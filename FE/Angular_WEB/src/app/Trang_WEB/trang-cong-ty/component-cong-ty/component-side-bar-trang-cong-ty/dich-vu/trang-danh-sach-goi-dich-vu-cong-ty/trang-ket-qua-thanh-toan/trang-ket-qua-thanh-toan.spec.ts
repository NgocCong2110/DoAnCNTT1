import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangKetQuaThanhToan } from './trang-ket-qua-thanh-toan';

describe('TrangKetQuaThanhToan', () => {
  let component: TrangKetQuaThanhToan;
  let fixture: ComponentFixture<TrangKetQuaThanhToan>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangKetQuaThanhToan]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangKetQuaThanhToan);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
