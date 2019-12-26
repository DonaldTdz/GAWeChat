import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'lottery-price-add',
  templateUrl: 'lottery-price-add.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryPriceAddComponent implements OnInit {

  input:any={
    name:Date,
    beginTime:Date,
    endTime:0
  };
   name:string="";
   num:number=0;
   type:string="";
  items: any[] = Array();

  itemGroup: any = [
    [
      {
        label: '一等奖',
        value: 1,
      },
      {
        label: '二等奖',
        disabled: true,
        value: 2,
      },
      {
        label: '三等奖',
        value: 3,
      },
    ]
  ]
  constructor(private router: Router, private actRouter: ActivatedRoute) { 

  }

  ngOnInit() {
  }

  save(){

    if(this.name!==""&&this.num!==0&&this.type!==""){
      //this.items.push({name:this.name,num:this.num,type:this.type});
      this.router.navigate(['lottery-draws/lottery-draw',{items:this.items,input:this.input}]);
    }
    this.router.navigate(['lottery-draws/lottery-draw',{items:this.items,input:this.input}]);

  }
 

}
