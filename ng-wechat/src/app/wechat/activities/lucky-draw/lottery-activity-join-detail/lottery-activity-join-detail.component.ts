import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';
import { Router, ActivatedRoute } from '@angular/router';
import { LotteryJoinDeptDetailOutput } from '../../../../services/model/lucky-draw';

@Component({
  selector: 'lottery-activity-join-detail',
  templateUrl: './lottery-activity-join-detail.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryActivityJoinDetailComponent implements OnInit {
  @Input() id: string;//活动详情Id
  @Input() deptName:string;//名字
  items:LotteryJoinDeptDetailOutput[]=[];//人员数据
  constructor(private lotterydrawService: LotteryService
    ,private router: Router
    ,private actRouter: ActivatedRoute) {
      this.id= this.actRouter.snapshot.params['id'];
      this.deptName= this.actRouter.snapshot.params['deptName'];
     }

  ngOnInit() {
    this.loadDeptEmployeeList();
  }
  //加载部门成员抽奖情况
  loadDeptEmployeeList(){
    this.lotterydrawService.getLotteryJoinDeptDetailAsync(this.id,this.deptName).subscribe(result => {
      console.log(result)
      this.items=result;
     });
  }

}
