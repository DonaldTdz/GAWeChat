import { Component, OnInit, ViewEncapsulation, Injector } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';
import { AppComponentBase } from '../../../components/app-component-base';
import { ToptipsService } from 'ngx-weui';
import { GetLuckySignInfoDto } from '../../../../services/model/lucky-sign';

@Component({
  selector: 'lottery-sign-in',
  templateUrl: 'lottery-sign-in.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotterySignInComponent extends AppComponentBase  implements OnInit {

  items:GetLuckySignInfoDto=new GetLuckySignInfoDto();//个人签到信息实体

  constructor(private router: Router, private actRouter: ActivatedRoute
    , private lotteryService: LotteryService
    ,injector: Injector
    , private srv: ToptipsService
    ) {
    super(injector);
  }

  ngOnInit() {
    this.onload();
  }

  //年会签到方法
  signIn(){
    this.lotteryService.getCreateWXLuckyDrawAsync(this.settingsService.openId).subscribe(result => {
      if(result.code==0){
        this.srv['success']('签到成功');
        this.onload();
      }else{
        this.srv['success'](result.msg);
      }
  }
  )
}

    ///加载个人信息
  onload(){
    console.log(this.settingsService.openId)
    this.lotteryService.getLuckySignInfoAsync(this.settingsService.openId).subscribe(result => {
      console.log(result)
      if(result.code==0){//成功
        this.items=result.data;

      }else if(result.code=901){
        this.srv['warn'](result.msg);
      }else if(result.code==902){
        this.srv['warn'](result.msg);
      }else{
        this.srv['warn']('服务器连接失败！');
      }
      
      

    })}
  
}
