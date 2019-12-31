import { Component, OnInit, ViewEncapsulation, ViewChild, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { timer } from 'rxjs/observable/timer';
import { map } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../services/article/lottery.service';
import { ToptipsService } from 'ngx-weui';
import { AppComponentBase } from '../../components/app-component-base';



@Component({
  selector: 'lottery-draw',
  templateUrl: 'lottery-draw.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryDrawComponent extends AppComponentBase implements OnInit {
  
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
    , injector: Injector
    , private lotteryService: LotteryService
    , private srv: ToptipsService
    ) { 
      super(injector);
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
     param.name=this.input.name;//活动名字
     param.endTime=this.input.endTime;//结束时间
     param.beginTime=this.input.beginTime;//开始时间
     param.list=this.items;//奖品列表
     param.isPublish=isPublish;//是否公布
     param.openId=this.settingsService.openId;//创建者ID
     console.log(param)
     this.lotteryService.CreateWXLuckyDrawAsync(param).subscribe(result => {
       console.log(result)
      if(result.code==0){
       
        if(isPublish){//并且发布活动
          console.log(result.data)
          this.lotteryService.getLotteryLogicAsync(result.data).subscribe(onSuccess => {
            if(onSuccess.code==0){
              this.srv['success'](onSuccess.msg);
              this.router.navigate(['/lotterys/lottery-activities-list']); 
            }else{
              this.srv['warn'](onSuccess.msg);
            }
          })
        }
      }else if(result.code=901){
        this.srv['warn']('创建活动出错！请重试！');
      }else{
        this.srv['warn']('服务器出错！');
      }      
     });
   }
  }
}
