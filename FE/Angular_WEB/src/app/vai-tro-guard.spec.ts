import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';
import { VaiTroGuard } from './vai-tro-guard';


describe('VaiTroGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
      TestBed.runInInjectionContext(() => VaiTroGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
