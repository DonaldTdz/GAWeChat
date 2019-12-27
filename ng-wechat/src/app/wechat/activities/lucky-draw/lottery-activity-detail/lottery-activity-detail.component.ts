import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';

@Component({
  selector: 'lottery-activity-detail',
  templateUrl: 'lottery-activity-detail.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryActivityDetailComponent implements OnInit {
  @Input() Id: string;//活动详情Id
  constructor(private actRouter: ActivatedRoute
    ,private lotteryService: LotteryService) {
      this.Id= this.actRouter.snapshot.params['id'];
   }

  ngOnInit() {
    this.getDetail();
  }

  items:any={};

  //获取活动详情
  getDetail(){
    this.lotteryService.getLuckyDrawDetailByIdAsync(this.Id).subscribe(result => {
      console.log(result);
      this.items=result;
    });
  }

  publish(){
    var input:any={};
    input.id=this.Id;
    this.lotteryService.updateWXLuckyDrawPubStatusAsync(input).subscribe(result => {
      this.getDetail();
    });
  }

}
