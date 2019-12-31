import { Component, OnInit, ViewEncapsulation, Injector, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AppComponentBase } from '../../../components/app-component-base';
import { ToptipsService, DialogConfig, DialogComponent, DialogService, ToastService } from 'ngx-weui';
import { GetLuckySignInfoDto } from '../../../../services/model/lucky-sign';
import { LotteryService, AppConsts } from '../../../../services';

@Component({
  selector: 'lottery-sign-in',
  templateUrl: 'lottery-sign-in.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotterySignInComponent extends AppComponentBase implements OnInit {
  @ViewChild('ios') iosAS: DialogComponent;
  @ViewChild('android') androidAS: DialogComponent;
  @ViewChild('auto') autoAS: DialogComponent;

  hostUrl: string = AppConsts.remoteServiceBaseUrl;
  items: GetLuckySignInfoDto = new GetLuckySignInfoDto();//个人签到信息实体
  isBind:boolean=true;//是否已经绑定
  isSigned:boolean;//是否签到

  //弹窗信息设置
  private DEFCONFIG: DialogConfig = {
    title: '请输入工号',
    content: '要参与签到，首先需要绑定工号，请在下方输入你的内部工号',
    cancel: '取消',
    confirm: '确认',
    inputPlaceholder: '请输入工号',
    inputError: '请填写工号',
    inputRequired: true,
    inputAttributes: {
      maxlength: 140,
      cn: 2,
    },
  } as DialogConfig;

  constructor(private router: Router, private actRouter: ActivatedRoute
    , private lotteryService: LotteryService
    , injector: Injector
    , private srv: ToptipsService
    , private src: DialogService
    , private toastService: ToastService
  ) {
    super(injector);
  }

  ngOnInit() {
    if (!this.settingsService.openId) {
      this.lotteryService.getAuthorizationUrl(this.hostUrl).subscribe((res) => {
        location.href = res;
      });
    } else {
      this.onload();
    }
  }

  //年会签到方法
  signIn() {
    this.lotteryService.getCreateWXLuckyDrawAsync(this.settingsService.openId).subscribe(result => {
      if (result.code == 0) {
        this.srv['success']('签到成功');
        this.onload();
      } else {
        this.srv['success'](result.msg);
      }
    });
  }

  ///加载个人信息
  onload() {
    this.lotteryService.getLuckySignInfoAsync(this.settingsService.openId).subscribe(result => {
      console.log(result)
      if (result.code === 0) {//成功
        this.items = result.data;
        this.isSigned=result.data.lotteryState
      } else if (result.code === 801) {
        this.isBind=false;
        this.bindJobNumber();
      }
      else {
        this.srv['warn'](result.msg);
      }
    });
  }

  //绑定工号
  bindJobNumber() {

    const cog = {
      ...this.DEFCONFIG,
      ...({
        skin: 'auto',
        type: 'prompt',
        confirm: '确认',
        cancel: '取消',
        input: 'text',
        inputValue: undefined,
        inputRegex: undefined,
      } as DialogConfig),
    } as DialogConfig;

    this.src.show(cog).subscribe((res: any) => {
      //res.result 输入文字
      console.log(res)
      if(res.value){
        //点击确认按钮
        var param:any={};
        param.openId=this.settingsService.openId;
        param.code=res.result;
        this.lotteryService.loterryBindWeChatUserAsync(param).subscribe(result => {
          console.log(result)
          if(result.code===0){
            this.srv['success']('绑定成功');
            this.isBind=true;
            this.onload();
          }else{
            this.srv['success'](result.msg);
          }
        })
      }else{//取消按钮

        
      }
    });
  }
}
