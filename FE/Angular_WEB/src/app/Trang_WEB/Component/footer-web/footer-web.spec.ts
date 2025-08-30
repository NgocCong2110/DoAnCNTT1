import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterWeb } from './footer-web';

describe('FooterWeb', () => {
  let component: FooterWeb;
  let fixture: ComponentFixture<FooterWeb>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FooterWeb]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FooterWeb);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
