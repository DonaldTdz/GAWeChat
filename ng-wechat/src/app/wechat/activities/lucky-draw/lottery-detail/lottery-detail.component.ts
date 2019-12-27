import { Component, OnInit, ViewEncapsulation, Input, Injector } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '../../../components/app-component-base';

@Component({
  selector: 'lottery-detail',
  templateUrl: 'lottery-detail.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryDetailComponent extends AppComponentBase implements OnInit {
  @Input() Id: string;//活动详情Id
  constructor(private lotteryService: LotteryService
    ,private router: Router
    ,private actRouter: ActivatedRoute
    ,injector: Injector,) {
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

  go


}
