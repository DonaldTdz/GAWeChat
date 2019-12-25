import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LotterySignInComponent } from './lottery-sign-in.component';

describe('LotterySignInComponent', () => {
  let component: LotterySignInComponent;
  let fixture: ComponentFixture<LotterySignInComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LotterySignInComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LotterySignInComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
