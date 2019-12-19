import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { DemandForecastServiceProxy } from '@shared/service-proxies/marketing-service';
import { DemandDetail, DemandForecast } from '@shared/entity/marketting';
import { DemandListComponent } from '../demand-list/demand-list.component';

@Component({
    moduleId: module.id,
    selector: 'demand-details',
    templateUrl: 'demand-details.component.html'
})
export class DemandDetailsComponent extends AppComponentBase implements OnInit {
    @ViewChild('itemList') itemList: DemandListComponent;

    id: string;
    form: FormGroup;
    loading = false;
    demandDetailList: DemandDetail[] = [];
    demandForecast: DemandForecast = new DemandForecast();
    cardTitle: string;
    successMsg = '';

    constructor(injector: Injector
        , private fb: FormBuilder
        , private router: Router
        , private modal: NzModalService
        , private actRouter: ActivatedRoute
        , private demandForecastService: DemandForecastServiceProxy) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }
    ngOnInit(): void {
        this.form = this.fb.group({
            title: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            month: [null, Validators.compose([Validators.required])],
        });
        this.getDemandForecastById();
    }
    getDemandForecastById() {
        if (this.id) {
            this.demandForecastService.get(this.id).subscribe((result: DemandForecast) => {
                this.demandForecast = result;
                this.itemList.id = this.demandForecast.id;
                this.itemList.isPublish = this.demandForecast.isPublish;
                this.itemList.refreshData();
                this.cardTitle = '需求预测详情';
            });
        } else {
            this.cardTitle = '新增需求预测';
        }
    }
    getFormControl(id: string) {
        return this.form.controls[id];
    }

    save() {
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.loading = true;
            // console.log(this.demandForecast);
            // this.demandForecast.month = this.dateFormatMM(this.demandForecast.month);
            this.demandForecast.month = this.dateFormatMM(this.demandForecast.month);
            // console.log(this.demandForecast);
            this.demandForecastService.update(this.demandForecast).subscribe((res) => {
                if (res.code == 0) {
                    this.successMsg = this.demandForecast.isPublish == false ? '保存成功！' : '发布成功！';
                    this.demandForecast = res.data;
                    this.itemList.id = this.demandForecast.id;
                    this.notify.info(this.successMsg, '');
                } else {
                    this.notify.error('保存失败，请重试！', '');

                }
                this.loading = false;
            });
        }
    }

    return() {
        this.router.navigate(['admin/marketting/demand-forecast']);
    }

    push() {
        //验证
        if (this.itemList.query.total == 0) {
            this.notify.error('请先导入需求预测数据');
            return;
        }
        this.modal.confirm({
            content: '发布后不可修改，是否确认发布?',
            cancelText: '否',
            okText: '是',
            onOk: () => {
                this.demandForecast.isPublish = true;
                this.save();
            }
        });
    }
}
