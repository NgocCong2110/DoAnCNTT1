import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CacSelector } from './cac-selector';

describe('CacSelector', () => {
  let component: CacSelector;
  let fixture: ComponentFixture<CacSelector>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CacSelector]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CacSelector);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
