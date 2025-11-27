import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentThongKeUngVienTungBai } from './component-thong-ke-ung-vien-tung-bai';

describe('ComponentThongKeUngVienTungBai', () => {
  let component: ComponentThongKeUngVienTungBai;
  let fixture: ComponentFixture<ComponentThongKeUngVienTungBai>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ComponentThongKeUngVienTungBai]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ComponentThongKeUngVienTungBai);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
