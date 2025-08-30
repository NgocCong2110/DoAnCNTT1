import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TinTuyenDung } from './tin-tuyen-dung';

describe('TinTuyenDung', () => {
  let component: TinTuyenDung;
  let fixture: ComponentFixture<TinTuyenDung>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TinTuyenDung]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TinTuyenDung);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
