import { Component, OnInit, ViewEncapsulation, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';
import { timer } from 'rxjs/observable/timer';
import { map } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';
import { LotteryService } from '../../../services/article/lottery.service';

@Component({
  moduleId: module.id,
  selector: 'lottery',
  templateUrl: 'lottery.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryComponent implements OnInit {


  input: any = {
    name: "",
    beginTime: Date,
    endTime: Date
  };

  items: any[] = Array();

  itemGroup: any = [
    [
      {
        label: '一等奖',
        value: 1,
      },
      {
        label: '二等奖',
        value: 2,
      },
      {
        label: '三等奖',
        value: 3,
      },
      {
        label: '四等奖',
        value: 4,
      },
      {
        label: '安慰奖',
        value: 5,
      },
      {
        label: '参与奖',
        value: 6,
      },
    ]
  ]

  constructor(private router: Router, private actRouter: ActivatedRoute
    , private lotteryService: LotteryService
  ) {
  }

  ngOnInit() {
    this.items.push({ name: "姓名1", num: 123, typeName: "一等奖", type: 1 });
  }


  onSendCode(): Observable<boolean> {
    return timer(1000).pipe(map(() => true));
  }


  iSPriceButtonShow: boolean = false;
  type: number = 0;
  typeName: string = "";
  name: string = "";
  num: number = 0;
  //存储奖品
  savePrice() {
    if (this.type !== 0 && this.name !== "" && this.num !== 0) {
      this.items.push({ name: this.name, num: this.num, type: this.type });
    }
    this.iSPriceButtonShow = false;
  }
  //添加更多奖品
  addMore() {
    this.iSPriceButtonShow = true;
    this.type = 0;
    this.name = "";
    this.num = 0;
  }
  closeMore() {
    this.iSPriceButtonShow = false;
  }

  //发布 --存储并公示
  publish() {

    this.submit(true);
  }
  loading: boolean;
  //保存表单
  onSave() {
    this.submit(false);
  }
  submit(isPublish: boolean) {

    var param: any = {};
    console.log("进入点击")
    if (this.input.Name !== "" && this.input.endTime !== null && this.input.beginTime !== null) {

      param.name = this.input.name;
      param.endTime = this.input.endTime;
      param.beginTime = this.input.beginTime;
      param.list = this.items;
      param.isPublish = isPublish;
      console.log(param)
      this.lotteryService.CreateWXLuckyDrawAsync(param).subscribe(result => {
        console.log(result);


        if (result["iSsuccess"] == 1) {
          this.router.navigate(['/lotterys/lottery-activities-list']);
        }
      });

    }
  }

}
