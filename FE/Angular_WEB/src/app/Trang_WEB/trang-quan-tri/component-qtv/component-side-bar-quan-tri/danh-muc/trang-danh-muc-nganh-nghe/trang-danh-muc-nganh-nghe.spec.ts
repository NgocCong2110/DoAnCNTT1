import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangDanhMucNganhNghe } from './trang-danh-muc-nganh-nghe';

describe('TrangDanhMucNganhNghe', () => {
  let component: TrangDanhMucNganhNghe;
  let fixture: ComponentFixture<TrangDanhMucNganhNghe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangDanhMucNganhNghe]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangDanhMucNganhNghe);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
