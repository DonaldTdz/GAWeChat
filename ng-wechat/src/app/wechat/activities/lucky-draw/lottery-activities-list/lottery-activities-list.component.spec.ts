import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LotteryActivitiesListComponent } from './lottery-activities-list.component';

describe('LotteryActivitiesListComponent', () => {
  let component: LotteryActivitiesListComponent;
  let fixture: ComponentFixture<LotteryActivitiesListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LotteryActivitiesListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LotteryActivitiesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
