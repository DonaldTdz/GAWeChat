import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';

@Component({
  selector: 'lottery-activities-list',
  templateUrl: 'lottery-activities-list.component.html',
  styleUrls: ['lottery-activities-list.component.less'],
  encapsulation: ViewEncapsulation.None,
})
export class LotteryActivitiesListComponent implements OnInit {

  constructor(private lotterydrawService: LotteryService) { }

  ngOnInit() {
    this.onload();
  }
  item:any[]=Array();

  onload(){

    this.lotterydrawService.GetWXLuckyDrawListAsyn().subscribe(result => {
       console.log(result);
       this.item=result;
     });
  }
  onLoadMore(){


  }

  publish(id:string){
    var input:any={};
    input.id=id;
    this.lotterydrawService.WXLuckyDrawUpdatePubStatusAsync(input).subscribe(result => {
      this.onload()
    });

  }
}
