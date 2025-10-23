import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheBanner } from './the-banner';

describe('TheBanner', () => {
  let component: TheBanner;
  let fixture: ComponentFixture<TheBanner>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TheBanner]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TheBanner);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
