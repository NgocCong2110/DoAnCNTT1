import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangCongTy } from './trang-cong-ty';

describe('TrangCongTy', () => {
  let component: TrangCongTy;
  let fixture: ComponentFixture<TrangCongTy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangCongTy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangCongTy);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
