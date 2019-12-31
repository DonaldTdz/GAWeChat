import { Component, OnInit, ViewEncapsulation, Injector, Input } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { Router, ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';
import { ToptipsService } from 'ngx-weui';
import { WXLuckyDrawDetailIDOutput, WeiXinPriceInput } from '../../../../services/model/lucky-draw';

@Component({
  selector: 'lottery-activity-edit',
  templateUrl: 'lottery-activity-edit.component.html',
  encapsulation: ViewEncapsulation.None,

})
export class LotteryActivityEditComponent extends AppComponentBase implements OnInit {
  @Input() id: string;//活动详情Id
  input:WXLuckyDrawDetailIDOutput=new WXLuckyDrawDetailIDOutput();//用于接收抽奖活动详细信息的类
  isSave:boolean=true;//是否保存
  prizeNum:number;//奖品数量
  prizeName:string;//奖品名称
  constructor(private router: Router, private actRouter: ActivatedRoute
    , injector: Injector
    , private lotteryService: LotteryService
    , private srv: ToptipsService) {
      super(injector);
      this.id= this.actRouter.snapshot.params['id'];
     }

  ngOnInit() {
    this.getDetail();
  }

    //获取活动详情
    getDetail(){
      this.lotteryService.getLuckyDrawDetailByIdAsync(this.id,this.settingsService.openId).subscribe(result => {
        console.log(result);
        this.input=result;
        this.prizeName=result.list[0].name,
        this.prizeNum=result.list[0].num
      });
    }

    //发布方法
    publish(){
      this.submit(true);
    }

    //更新方法
    update(){
      this.submit(false);
    }

     //统一表单提交函数
  submit(isPublish:boolean){

    var param:any={};
    console.log(new Date());

    if(this.input.endTime<this.input.beginTime){
      this.srv['warn']('活动结束时间不能大于开始时间');
      return;
    }

    if(new Date(this.input.endTime)<new Date()){
      this.srv['warn']('活动结束时间不能小于当前时间');
      return;
    }

    if(this.input.endTime!==null&&this.input.beginTime!==null){
      var list:any={};//奖品
      list.name=this.prizeName;//奖品名字
      list.num=this.prizeNum;//奖品数量
     param.name=this.input.name;//活动名字
     param.endTime=this.input.endTime;//结束时间
     param.beginTime=this.input.beginTime;//开始时间
     param.isPublish=isPublish;//是否公布
     param.list=list;//奖品
     param.openId=this.settingsService.openId;//创建者ID
     param.id=this.id;//主键ID
     console.log(param)
     this.lotteryService.updateWXLuckyDrawAsync(param).subscribe(result => {
       console.log(result)
      if(result.code==0){
       
        if(isPublish){//并且发布活动
              this.isSave=false;
              this.srv['success']("发布成功！");
              this.router.navigate(['/lotterys/lottery-activities-list']);      
        }
              this.isSave=false;
              this.srv['success']("更新成功！");
              this.router.navigate(['/lotterys/lottery-activities-list']); 

      }else if(result.code=901){
        this.srv['warn']('创建活动出错！请重试！');
      }else{
        this.srv['warn']('服务器出错！');
      }      
     });
   }
  }
  
}
