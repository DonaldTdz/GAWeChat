import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { LotteryService } from '../../../../services/article/lottery.service';
import { Router } from '@angular/router';

@Component({
  selector: 'lottery-activities-list',
  templateUrl: 'lottery-activities-list.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryActivitiesListComponent implements OnInit {

  constructor(private lotterydrawService: LotteryService
    ,private router: Router
    ) { }

  ngOnInit() {
    this.onload();
  }
  item:any[]=Array();

  onload(){

    this.lotterydrawService.GetWXLuckyDrawListAsync().subscribe(result => {
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
  goToDetail(id:string){
    this.router.navigate(['/lotterys/lottery-activity-detail',{id:id}]);
  }
}
