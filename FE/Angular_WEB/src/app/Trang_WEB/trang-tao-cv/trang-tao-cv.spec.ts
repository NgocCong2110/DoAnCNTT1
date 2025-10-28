import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangTaoCv } from './trang-tao-cv';

describe('TrangTaoCv', () => {
  let component: TrangTaoCv;
  let fixture: ComponentFixture<TrangTaoCv>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangTaoCv]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangTaoCv);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
