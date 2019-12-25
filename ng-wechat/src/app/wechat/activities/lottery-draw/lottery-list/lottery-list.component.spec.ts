import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LotteryListComponent } from './lottery-list.component';

describe('LotteryListComponent', () => {
  let component: LotteryListComponent;
  let fixture: ComponentFixture<LotteryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LotteryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LotteryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
