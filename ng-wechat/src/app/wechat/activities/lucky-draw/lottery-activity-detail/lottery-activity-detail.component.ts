import { Component, OnInit, ViewEncapsulation, Input, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';
import { AppComponentBase } from '../../../components/app-component-base';

@Component({
  selector: 'lottery-activity-detail',
  templateUrl: 'lottery-activity-detail.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryActivityDetailComponent extends AppComponentBase implements OnInit {
  @Input() Id: string;//活动详情Id
  constructor(private actRouter: ActivatedRoute
    ,private lotteryService: LotteryService
    ,injector: Injector) {
    super(injector);
      this.Id= this.actRouter.snapshot.params['id'];
   }

  ngOnInit() {
    this.getDetail();
  }

  items:any={};

  //获取活动详情
  getDetail(){
    this.lotteryService.getLuckyDrawDetailByIdAsync(this.Id,this.settingsService.openId).subscribe(result => {
      console.log(result);
      this.items=result;
    });
  }

  publish(){
    var input:any={};
    input.id=this.Id;
    console.log(input)
    this.lotteryService.updateWXLuckyDrawPubStatusAsync(input).subscribe(result => {
      console.log(result)
      this.getDetail();
    });
  }

}
