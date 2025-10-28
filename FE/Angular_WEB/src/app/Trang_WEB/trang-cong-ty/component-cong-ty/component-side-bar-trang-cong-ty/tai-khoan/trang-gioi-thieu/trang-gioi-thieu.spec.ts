import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrangGioiThieu } from './trang-gioi-thieu';

describe('TrangGioiThieu', () => {
  let component: TrangGioiThieu;
  let fixture: ComponentFixture<TrangGioiThieu>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrangGioiThieu]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrangGioiThieu);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
