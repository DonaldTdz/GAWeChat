import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';
import { Router } from '@angular/router';

@Component({
  selector: 'lottery-sign-in-list',
  templateUrl: 'lottery-sign-in-list.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotterySignInListComponent implements OnInit {

  constructor(private lotterydrawService: LotteryService,private router: Router) { }

  ngOnInit() {
    this.loadlist();


  }

  time: number;
  onSelect() {
    this.time = new Date().getTime();
  }
  items: any[] = Array();

  loadlist(){
    this.lotterydrawService.getEmployeeNameListAsyn().subscribe(result => {
     this.items=result;
    });
  }
  onLoadMore(event:any){

  }
  ///查看部门详细人员
  goToDetail(i:string){
    console.log(i)
     this.router.navigate(['/lotterys/lottery-sign-in-detail',{deptName:i}]);
  }


  
  
}
