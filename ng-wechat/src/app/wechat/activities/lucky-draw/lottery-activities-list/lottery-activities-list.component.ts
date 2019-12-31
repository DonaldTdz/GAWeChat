import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';
import { Router } from '@angular/router';
import { ToptipsService } from 'ngx-weui';
import { LuckyDraw } from '../../../../services/model/lucky-draw';

@Component({
  selector: 'lottery-activities-list',
  templateUrl: 'lottery-activities-list.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryActivitiesListComponent implements OnInit {

  item:LuckyDraw[]=[];//管理员活动列表
  constructor(private lotterydrawService: LotteryService
    ,private router: Router
    , private srv: ToptipsService
    ) { }

  ngOnInit() {
    this.onload();
  }
  onload(){

    this.lotterydrawService.getWXLuckyDrawListAsync().subscribe(result => {
      this.item=result;
  
     });
  }
  onLoadMore(){


  }

  publish(id:string){
    var input:any={};
    input.id=id;
    this.lotterydrawService.updateWXLuckyDrawPubStatusAsync(input).subscribe(result => {
      if(result.code==0){
        this.srv['success'](result.msg);
       }else if(result.code==901){
        this.srv['warn'](result.msg);
       }else if(result.code==902){
        this.srv['warn'](result.msg);
       }else{
        this.srv['warn']('服务器错误');
       }
      this.onload()
    });

  }
  goToDetail(id:string){
    this.router.navigate(['/lotterys/lottery-activity-detail',{id:id}]);
  }
}
