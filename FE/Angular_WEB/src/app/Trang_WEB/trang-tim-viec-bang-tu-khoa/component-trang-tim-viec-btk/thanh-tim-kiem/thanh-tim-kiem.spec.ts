import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThanhTimKiem } from './thanh-tim-kiem';

describe('ThanhTimKiem', () => {
  let component: ThanhTimKiem;
  let fixture: ComponentFixture<ThanhTimKiem>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ThanhTimKiem]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThanhTimKiem);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
