import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HeaderWEB } from './header-web';

describe('HeaderWEB', () => {
  let component: HeaderWEB;
  let fixture: ComponentFixture<HeaderWEB>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderWEB]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HeaderWEB);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
