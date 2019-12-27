import { Component, OnInit, ViewEncapsulation, Injector } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../../services/article/lottery.service';
import { AppComponentBase } from '../../../components/app-component-base';

@Component({
  selector: 'lottery-sign-in',
  templateUrl: 'lottery-sign-in.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotterySignInComponent extends AppComponentBase  implements OnInit {

  items:any={};

  constructor(private router: Router, private actRouter: ActivatedRoute
    , private lotteryService: LotteryService
    ,injector: Injector,
    ) {
    super(injector);
  }

  ngOnInit() {
    this.onload();
  }

  //年会签到方法
  signIn(){

  //   this.lotteryService.getLuckySignInfoAsync(this.settingsService.openId).subscribe(result => {
  //     console.log(result);
  // }
  //)
}

    ///加载个人信息
  onload(){
    console.log(this.settingsService.openId)
    this.lotteryService.getLuckySignInfoAsync(this.settingsService.openId).subscribe(result => {
      console.log(result);
      this.items=result;

    })}
  
}
