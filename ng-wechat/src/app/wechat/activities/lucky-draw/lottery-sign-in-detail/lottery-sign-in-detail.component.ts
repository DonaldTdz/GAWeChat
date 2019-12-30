import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';
import { GetEmployeeDetailByDeptOutput } from '../../../../services/model/lucky-draw';

@Component({
  selector: 'lottery-sign-in-detail',
  templateUrl: 'lottery-sign-in-detail.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotterySignInDetailComponent implements OnInit {
  @Input() deptName:string;//部门名字
  items: GetEmployeeDetailByDeptOutput[] = [];//名字，工号，是否签到信息数组
  keyWord:string="";//关键词
  departmentId:string="";//部门ID
  constructor(
     private actRouter: ActivatedRoute
    ,private lotterydrawService: LotteryService
  ) { 

     this.deptName= this.actRouter.snapshot.params['deptName'];
  }
  //加载列表
  loadlistByName(){
    
    this.lotterydrawService.getSignListByDeptNameAsync(this.deptName).subscribe(result => {
      this.items=result;
     });

  }
  ngOnInit() {
    this.loadlistByName();
  }
}
