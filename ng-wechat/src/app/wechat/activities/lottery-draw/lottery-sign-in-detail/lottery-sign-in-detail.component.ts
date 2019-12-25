import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'lottery-sign-in-detail',
  templateUrl: './lottery-sign-in-detail.component.html',
  styleUrls: ['./lottery-sign-in-detail.component.less']
})
export class LotterySignInDetailComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    this.items.push({name:"姓名1",telephone:"153020160",licenseKey:"是"});
    this.items.push({name:"姓名1",telephone:"153020160",licenseKey:"否"});
    this.items.push({name:"姓名1",telephone:"153020160",licenseKey:"是"});
    this.items.push({name:"姓名1",telephone:"153020160",licenseKey:"是"});
    this.items.push({name:"姓名1",telephone:"153020160",licenseKey:"否"});
    this.items.push({name:"姓名1",telephone:"153020160",licenseKey:"是"});
  }
  onSelect() {

  }
  items: any[] = Array();

  keyWord:string="";//关键词
  departmentId:string="";//部门ID
  //员工过滤方法
  getStaff(filter){

    this.keyWord=filter;


  }

}
