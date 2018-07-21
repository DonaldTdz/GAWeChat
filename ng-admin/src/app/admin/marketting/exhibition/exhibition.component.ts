import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ExhibitionShopServiceProxy, PagedResultDtoOfExhibitionShop } from '@shared/service-proxies/marketing-service';
import { Parameter } from '@shared/service-proxies/entity';
import { Router } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { ExhibitionShop, Exhibition } from '@shared/entity/marketting';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import * as moment from 'moment';
import { AppConsts } from '@shared/AppConsts';

@Component({
    moduleId: module.id,
    selector: 'exhibition',
    templateUrl: 'exhibition.component.html',
})
export class ExhibitionComponent extends AppComponentBase implements OnInit {
    exhibitionShopList: ExhibitionShop[] = [];
    loading = false;
    exportLoading = false;
    search: any = {};
    exhibition: Exhibition = new Exhibition();
    form: FormGroup;
    sortMap = {
        votes: null,
        fans: null
    };
    sortVotesTotal = null;
    sortFansTotal = null;

    constructor(injector: Injector, private fb: FormBuilder,
        private router: Router, private modal: NzModalService,
        private exhibitionService: ExhibitionShopServiceProxy, ) {
        super(injector);
    }
    ngOnInit(): void {
        this.form = this.fb.group({
            topTotal: [null, [Validators.compose([Validators.required, Validators.pattern(/^[+]{0,1}(\d+)$|^[+]{0,1}(\d+\.\d+)$/), Validators.min(0)])]],
            frequency: [null, [Validators.compose([Validators.required, Validators.pattern(/^[+]{0,1}(\d+)$|^[+]{0,1}(\d+\.\d+)$/), Validators.min(0)])]],
            desc: [null],
            // content: [null],
            endTime: [null, Validators.compose([Validators.required])],
            beginTime: [null, Validators.compose([Validators.required])],
        });
        this.refreshData();
        this.getExhibitionConfig();
    }

    getFormControl(id: string) {
        return this.form.controls[id];
    }

    sort(value, para: string) {
        if (para == 'fans') {
            this.sortFansTotal = value;
            this.sortVotesTotal = null;
            this.sortMap.votes = null;
            this.refreshData();
        } else {
            this.sortVotesTotal = value;
            this.sortFansTotal = null;
            this.sortMap.fans = null;
            this.refreshData();
        }
    }
    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = {};
            this.sortVotesTotal = null;
            this.sortFansTotal = null;
            this.sortMap = {
                fans: null,
                votes: null
            };
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.exhibitionService.getAll(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfExhibitionShop) => {
            this.loading = false;
            this.exhibitionShopList = result.items;
            this.query.total = result.totalCount;
        })
    }

    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'ShopName', value: this.search.shopName }));
        arry.push(Parameter.fromJS({ key: 'CustName', value: this.search.custName }));
        arry.push(Parameter.fromJS({ key: 'CustCode', value: this.search.custCode }));
        arry.push(Parameter.fromJS({ key: 'Phone', value: this.search.phone }));
        arry.push(Parameter.fromJS({ key: 'sortFansTotal', value: this.sortFansTotal }));
        arry.push(Parameter.fromJS({ key: 'sortVotesTotal', value: this.sortVotesTotal }));
        return arry;
    }

    exhibitionDetails(id: string) {
        this.router.navigate(['admin/marketting/exhibition-detail', id]);
    }
    getExhibitionConfig() {
        this.exhibitionService.get().subscribe((result: Exhibition) => {
            this.exhibition = result;
        });
    }
    save() {
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.loading = true;
            this.exhibition.beginTime = this.dateFormat(this.exhibition.beginTime);
            this.exhibition.endTime = this.dateFormat(this.exhibition.endTime);
            this.exhibitionService.update(this.exhibition).subscribe(() => {
                this.notify.info('保存成功！');
                this.getExhibitionConfig();
                this.loading = false;
            });
        }
    }
    exportExcel() {
        this.exportLoading = true;
        this.exhibitionService.exportExcel({ shopName: this.search.shopName, custName: this.search.custName, custCode: this.search.custCode, phone: this.search.phone, sortFansTotal: this.sortFansTotal, sortVotesTotal: this.sortVotesTotal }).subscribe(result => {
            if (result.code == 0) {
                var url = AppConsts.remoteServiceBaseUrl + result.data;
                document.getElementById('aExhibitionShopExcelUrl').setAttribute('href', url);
                document.getElementById('btnExhibitionShopHref').click();
            } else {
                this.notify.error(result.msg);
            }
            this.exportLoading = false;
        });
    }
}
