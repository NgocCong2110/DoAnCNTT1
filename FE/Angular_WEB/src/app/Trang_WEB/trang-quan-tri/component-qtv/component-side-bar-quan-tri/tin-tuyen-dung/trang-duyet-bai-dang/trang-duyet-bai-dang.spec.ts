import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDuyetBaiDang } from './trang-duyet-bai-dang';

describe('TrangDuyetBaiDang', () => {
  let component: TrangDuyetBaiDang;
  let fixture: ComponentFixture<TrangDuyetBaiDang>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDuyetBaiDang]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDuyetBaiDang);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
