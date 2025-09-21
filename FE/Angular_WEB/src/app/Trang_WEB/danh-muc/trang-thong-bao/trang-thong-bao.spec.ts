import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangThongBao } from './trang-thong-bao';

describe('TrangThongBao', () => {
  let component: TrangThongBao;
  let fixture: ComponentFixture<TrangThongBao>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangThongBao]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangThongBao);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
