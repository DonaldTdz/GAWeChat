import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';
import { LuckyDraw } from '../../../../services/model/lucky-draw';

@Component({
  selector: 'lottery-list',
  templateUrl: 'lottery-list.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryListComponent implements OnInit {
  items:LuckyDraw[]=[];//内部员工获取抽奖列表实体集合
  constructor(private lotteryService: LotteryService
    ,private router: Router) { }
  
  ngOnInit() {
    this.onload();
  }
  //加载列表
  onload(){
    this.lotteryService.getWXLuckyDrawListPublishedAsync().subscribe(result => {
    this.items=result;
    });
  }

  //进入抽奖详情页并参与抽奖
  goToJoin(id:string){
    this.router.navigate(['/lotterys/lottery-detail',{id:id}]);
  }
}
