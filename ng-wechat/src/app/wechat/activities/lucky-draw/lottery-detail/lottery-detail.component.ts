import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'lottery-detail',
  templateUrl: 'lottery-detail.component.html',
  styleUrls: ['lottery-detail.component.less'],
  encapsulation: ViewEncapsulation.None,
})
export class LotteryDetailComponent implements OnInit {
  @Input() Id: string;//活动详情Id
  constructor(private lotteryService: LotteryService
    ,private router: Router
    ,private actRouter: ActivatedRoute) {
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


}
