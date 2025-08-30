import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThongTinNoiBat } from './thong-tin-noi-bat';

describe('ThongTinNoiBat', () => {
  let component: ThongTinNoiBat;
  let fixture: ComponentFixture<ThongTinNoiBat>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ThongTinNoiBat]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThongTinNoiBat);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
