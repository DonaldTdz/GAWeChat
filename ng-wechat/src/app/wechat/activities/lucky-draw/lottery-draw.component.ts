import { Component, OnInit, ViewEncapsulation, Injector } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../services/article/lottery.service';
import { ToptipsService } from 'ngx-weui';
import { AppComponentBase } from '../../components/app-component-base';



@Component({
  selector: 'lottery',
  templateUrl: 'lottery.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryComponent extends AppComponentBase implements OnInit {

  prizeNum: number//奖品数量
  prizeName: string;//奖品名字
  name: string;//活动名字
  loading: boolean;//按钮加载
  isSave: boolean = true;//是否保存

  //表单数据
  input: any = {
    name: "",//活动名字
    beginTime: Date,//开始时间
    endTime: Date//结束时间
  };
  constructor(private router: Router, private actRouter: ActivatedRoute
    , injector: Injector
    , private lotteryService: LotteryService
    , private srv: ToptipsService
  ) {
    super(injector);
  }

  ngOnInit() {
    this.input.beginTime = null;
    this.input.endTime = null;
  }

  //发布 --存储并公示
  publish() {
    this.submit(true);
  }

  //保存表单
  onSave() {
    console.log(this.input)
    this.submit(false);
  }
  //统一表单提交函数
  submit(isPublish: boolean) {

    var param: any = {};
    console.log(new Date())
    if (this.input.endTime < this.input.beginTime) {
      this.srv['warn']('活动结束时间不能大于开始时间');
      return;
    }

    if (new Date(this.input.endTime) < new Date()) {
      this.srv['warn']('活动结束时间不能小于当前时间');
      return;
    }

    if (this.input.endTime !== null && this.input.beginTime !== null) {
      var list: any = {};
      list.name = this.prizeName,//奖品名字
        list.num = this.prizeNum,//奖品数量
        param.name = this.input.name;//活动名字
      param.endTime = this.input.endTime;//结束时间
      param.beginTime = this.input.beginTime;//开始时间
      param.list = list;//奖品列表
      param.isPublish = isPublish;//是否公布
      param.openId = this.settingsService.openId;//创建者ID
      console.log(param)
      this.lotteryService.CreateWXLuckyDrawAsync(param).subscribe(result => {
        console.log(result)
        if (result.code == 0) {

          if (isPublish) {//并且发布活动
            this.isSave = false;
            this.srv['success'](result.msg);
            this.router.navigate(['/lotterys/lottery-activities-list']);
          }
          this.isSave = false;
          this.srv['success']("保存成功！");
          this.router.navigate(['/lotterys/lottery-activities-list']);

        } else if (result.code = 901) {
          this.srv['warn']('创建活动出错！请重试！');
        } else {
          this.srv['warn']('服务器出错！');
        }
      });
    }
  }
}
