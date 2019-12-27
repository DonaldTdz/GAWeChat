import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';

@Component({
  selector: 'lottery-sign-in-detail',
  templateUrl: 'lottery-sign-in-detail.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotterySignInDetailComponent implements OnInit {
  @Input() deptName:string;
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
  onSelect() {

  }
  items: any[] = Array();

  keyWord:string="";//关键词
  departmentId:string="";//部门ID
  //员工过滤方法
  getStaff(filter){

    this.keyWord=filter;


  }
  onLoadMore(event:any){
    
  }

}
