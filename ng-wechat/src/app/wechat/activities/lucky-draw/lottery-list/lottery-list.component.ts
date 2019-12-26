import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';

@Component({
  selector: 'lottery-list',
  templateUrl: 'lottery-list.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryListComponent implements OnInit {

  constructor(private lotteryService: LotteryService
    ,private router: Router) { }

  ngOnInit() {
    this.onload();
  }
  items:any=[];

  //加载列表
  onload(){
    this.lotteryService.GetWXLuckyDrawListPublishedAsync().subscribe(result => {
      console.log(result);
      this.items=result;
    });
  }

  //进入抽奖详情页并参与抽奖
  goToJoin(id:string){
    console.log("11")
    this.router.navigate(['/lotterys/lottery-detail',{id:id}]);
  }

}
