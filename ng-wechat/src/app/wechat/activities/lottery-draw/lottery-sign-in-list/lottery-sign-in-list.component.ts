import { Component, OnInit } from '@angular/core';
import { InfiniteLoaderComponent } from 'ngx-weui';

@Component({
  selector: 'lottery-sign-in-list',
  templateUrl: './lottery-sign-in-list.component.html',
  styleUrls: ['./lottery-sign-in-list.component.less']
})
export class LotterySignInListComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    this.items.push('开发部');
    this.items.push('销售部');
    this.items.push('财务部');
    this.items.push('实施部');
    this.items.push('会账部');
    this.items.push('测试部');


  }

  time: number;
  onSelect() {
    this.time = new Date().getTime();
  }
  items: any[] = Array()


  
  
}
