import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LotteryJoinComponent } from './lottery-join.component';

describe('LotteryJoinComponent', () => {
  let component: LotteryJoinComponent;
  let fixture: ComponentFixture<LotteryJoinComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LotteryJoinComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LotteryJoinComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
