import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';
import { Router } from '@angular/router';
import { NumberValueAccessor } from '@angular/forms/src/directives';

@Component({
  selector: 'lottery-sign-in-list',
  templateUrl: 'lottery-sign-in-list.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotterySignInListComponent implements OnInit {
  items: string[] = [];//字符串列表用以接收部门名字数据
  num_Total:number;//总人数
  num_UnSign:number;//未签到人数
  constructor(private lotterydrawService: LotteryService,private router: Router) { }
  ngOnInit() {
    this.loadlist();
    this.getSignInNum();
  }


  loadlist(){
    this.lotterydrawService.getEmployeeNameListAsyn().subscribe(result => {
     this.items=result;
    });
  }
  ///查看部门详细人员
  goToDetail(i:string){
    console.log(i)
     this.router.navigate(['/lotterys/lottery-sign-in-detail',{deptName:i}]);
  } 
    //获取签到人数
    getSignInNum(){
      this.lotterydrawService.getSignInPeronNumAsync().subscribe(result => {
        console.log(result)
       if(result.code==0){
         this.num_Total=result.data.num_Total;
         this.num_UnSign=result.data.num_UnSign;
       }
      });
    }
  
}
