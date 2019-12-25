import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Observable } from 'rxjs';
import { timer } from 'rxjs/observable/timer';
import { map } from 'rxjs/operators';
import { Uploader } from 'ngx-weui';
import { Router } from '@angular/router';

@Component({
  selector: 'lottery-draw',
  templateUrl: './lottery-draw.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class LotteryDrawComponent implements OnInit {
  res: any = {
    cho2: true,
    worldpost: '1',
    contact: '1',
    country: '1',
    agree: true,
  };

  input:any={
    price_name:"",
    price_count:0,
  };

  uploader: Uploader = new Uploader({
    url: './upload.php',
    headers: [{ name: 'auth', value: 'test' }],
    params: {
      a: 1,
      b: new Date(),
      c: 'test',
      d: 12.123,
    },})
  itemGroup: any = [
    [
      {
        label: '一等奖',
        value: 1,
      },
      {
        label: '二等奖',
        disabled: true,
        value: 2,
      },
      {
        label: '三等奖',
        value: 3,
      },
    ]
  ]

  radio: any[] = [{ id: 1, name: 'asdf1' }, { id: 2, name: 'asdf2' }];
  checkbox: any[] = ['A', 'B'];

  constructor(private router: Router,) { 
    this.res.radio = this.radio[0];
    this.res.checkbox = [this.checkbox[0]];
  }

  ngOnInit() {
  }
  onAddCheckbox() {
    // tslint:disable-next-line: binary-expression-operand-order
    this.checkbox.push(String.fromCharCode(65 + this.checkbox.length));
  }

  onSendCode(): Observable<boolean> {
    return timer(1000).pipe(map(() => true));
  }

  onSave() {
    alert('请求数据：' + JSON.stringify(this.res));
  }
  //添加更多奖品
  addMore(){
    this.router.navigate(['lottery-draws/lottery-price-add']);
  }

}
