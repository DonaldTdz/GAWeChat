import { Component, OnInit, ViewEncapsulation, Input, Injector } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';
import { AppComponentBase } from '../../../components/app-component-base';
import { WXLuckyDrawDetailIDOutput } from '../../../../services/model/lucky-draw';

@Component({
  selector: 'lottery-activity-detail',
  templateUrl: 'lottery-activity-detail.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryActivityDetailComponent extends AppComponentBase implements OnInit {
  @Input() id: string;//活动详情Id
  items:WXLuckyDrawDetailIDOutput=new WXLuckyDrawDetailIDOutput();//用于接收抽奖活动详细信息的类

  constructor(private actRouter: ActivatedRoute
    ,private lotteryService: LotteryService
    ,private router: Router
    ,injector: Injector) {
    super(injector);
      this.id= this.actRouter.snapshot.params['id'];
   }

  ngOnInit() {
    this.getDetail();
  }


  //获取活动详情
  getDetail(){
    this.lotteryService.getLuckyDrawDetailByIdAsync(this.id,this.settingsService.openId).subscribe(result => {
      console.log(result);
      this.items=result;
    });
  }

  //公布该活动
  publish(){
    var input:any={};
    input.id=this.id;
    console.log(input)
    this.lotteryService.updateWXLuckyDrawPubStatusAsync(input).subscribe(result => {
      console.log(result)
      this.getDetail();
    });
  }
  //展示参与抽奖的部门和人员列表
  shoWLotteryJoinList(){

     this.router.navigate(['/lotterys/lottery-activity-join-list',{id:this.id,name:this.items.name}]);
  }

}
