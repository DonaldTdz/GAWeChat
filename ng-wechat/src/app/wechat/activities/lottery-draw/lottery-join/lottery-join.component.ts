import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'lottery-join',
  templateUrl: './lottery-join.component.html',
  styleUrls: ['./lottery-join.component.less'],
  encapsulation: ViewEncapsulation.None,
})
export class LotteryJoinComponent implements OnInit {
  txt:string="";
  vcode:string="";
  constructor() { }

  ngOnInit() {
  }

  show: boolean = false;


}
