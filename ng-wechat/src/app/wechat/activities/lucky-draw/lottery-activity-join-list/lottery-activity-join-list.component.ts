import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'lottery-activity-join-list',
  templateUrl: 'lottery-activity-join-list.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryActivityJoinListComponent implements OnInit {
  @Input() id: string;//活动详情Id
  @Input() name:string;//名字
  items:string[]=[];//部门名字列表
  num_Total:number;//总人数
  num_Lottery:number;//参与人数
  constructor(private lotterydrawService: LotteryService,private router: Router,private actRouter: ActivatedRoute) {
    this.id= this.actRouter.snapshot.params['id'];
    this.name= this.actRouter.snapshot.params['name'];
   }
  ngOnInit() {
    this.loadlist();
    this.loadLotteryStatistics();
  }
  //加载部门列表
  loadlist(){
    this.lotterydrawService.getLuckyDeptmentLotteryPersonAsync(this.id).subscribe(result => {
      console.log(result)
     this.items=result;
    });
  }
  //根据部门查看抽奖列表
  goToDetail(i:string){
    console.log(i)
     this.router.navigate(['/lotterys/lottery-activity-join-detail',{deptName:i,id:this.id}]);
  } 

  //获取参与总人数
  loadLotteryStatistics(){
    this.lotterydrawService.getLuckyDrawPersonCountAsync(this.id).subscribe(result => {
      this.num_Total=result.num_Total;
      this.num_Lottery=result.num_Lottery;
  })
}
}
