import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LotteryPriceAddComponent } from './lottery-price-add.component';

describe('LotteryPriceAddComponent', () => {
  let component: LotteryPriceAddComponent;
  let fixture: ComponentFixture<LotteryPriceAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LotteryPriceAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LotteryPriceAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
