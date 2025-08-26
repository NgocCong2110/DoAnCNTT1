import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangChu } from './trang-chu';

describe('TrangChu', () => {
  let component: TrangChu;
  let fixture: ComponentFixture<TrangChu>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangChu]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangChu);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
