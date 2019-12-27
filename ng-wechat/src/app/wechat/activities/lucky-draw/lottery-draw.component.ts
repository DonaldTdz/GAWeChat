import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { timer } from 'rxjs/observable/timer';
import { map } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../services/article/lottery.service';
import { ToptipsService } from 'ngx-weui';



@Component({
  selector: 'lottery-draw',
  templateUrl: 'lottery-draw.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryDrawComponent implements OnInit {
  
  iSPriceButtonShow:boolean=false;//奖品编辑按钮是否显示
  type:number=0;//奖品等级
  typeName:string="";//奖品名字
  name:string="";//活动名字
  num:number=0;//奖品数量
  loading:boolean;//按钮加载

  //表单数据
  input:any={
    name:"",//活动名字
    beginTime:Date,//开始时间
    endTime:Date//结束时间
  };

  items: any[] = Array();//奖品列表


  constructor(private router: Router, private actRouter: ActivatedRoute
    , private lotteryService: LotteryService
    , private srv: ToptipsService
    ) { 
  }

  ngOnInit() {
   
  }
 
  //存储奖品
  savePrice(){
      if(this.name!==""&&this.num!==0){
    this.items.push({name:this.name,num:this.num});
  }
  this.iSPriceButtonShow=false; 
  }
  //添加更多奖品
  addMore(){   
    this.iSPriceButtonShow=true;
    this.type=0;
    this.name="";
    this.num=0;
  }
  //关闭奖品添加
  closeMore(){
    this.iSPriceButtonShow=false;
  }

  //发布 --存储并公示
  publish(){
     this.submit(true);
  }
 
  //保存表单
  onSave() {
    this.submit(false);
  }
  //统一表单提交函数
  submit(isPublish:boolean){

    var param:any={};
    console.log("进入点击")
    if(this.input.endTime!==null&&this.input.beginTime!==null){
     param.name=this.input.name;
     param.endTime=this.input.endTime;
     param.beginTime=this.input.beginTime;
     param.list=this.items;
     param.isPublish=isPublish;
     console.log(param)
     this.lotteryService.CreateWXLuckyDrawAsync(param).subscribe(result => {
       console.log(result)
      if(result.code==0){
        this.srv['success']('活动创建成功！');
        this.router.navigate(['/lotterys/lottery-activities-list']); 
      }else if(result.code=901){
        this.srv['warn']('创建活动出错！请重试！');
      }else{
        this.srv['success']('服务器出错！');
      }      
     });
   }
  }
}
