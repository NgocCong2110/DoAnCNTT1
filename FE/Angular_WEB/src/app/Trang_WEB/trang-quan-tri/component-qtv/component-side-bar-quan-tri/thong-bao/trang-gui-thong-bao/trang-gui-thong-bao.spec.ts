import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangGuiThongBao } from './trang-gui-thong-bao';

describe('TrangGuiThongBao', () => {
  let component: TrangGuiThongBao;
  let fixture: ComponentFixture<TrangGuiThongBao>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangGuiThongBao]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangGuiThongBao);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
