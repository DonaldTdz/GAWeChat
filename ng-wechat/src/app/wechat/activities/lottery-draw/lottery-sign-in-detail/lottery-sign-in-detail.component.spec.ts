import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LotterySignInDetailComponent } from './lottery-sign-in-detail.component';

describe('LotterySignInDetailComponent', () => {
  let component: LotterySignInDetailComponent;
  let fixture: ComponentFixture<LotterySignInDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LotterySignInDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LotterySignInDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
