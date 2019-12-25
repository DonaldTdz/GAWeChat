import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LotterySignInListComponent } from './lottery-sign-in-list.component';

describe('LotterySignInListComponent', () => {
  let component: LotterySignInListComponent;
  let fixture: ComponentFixture<LotterySignInListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LotterySignInListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LotterySignInListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
