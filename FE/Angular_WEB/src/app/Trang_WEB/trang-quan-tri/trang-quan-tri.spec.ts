import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangQuanTri } from './trang-quan-tri';

describe('TrangQuanTri', () => {
  let component: TrangQuanTri;
  let fixture: ComponentFixture<TrangQuanTri>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangQuanTri]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangQuanTri);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
