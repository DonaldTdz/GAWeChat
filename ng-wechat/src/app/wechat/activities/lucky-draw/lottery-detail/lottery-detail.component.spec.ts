import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LotteryDetailComponent } from './lottery-detail.component';

describe('LotteryDetailComponent', () => {
  let component: LotteryDetailComponent;
  let fixture: ComponentFixture<LotteryDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LotteryDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LotteryDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
