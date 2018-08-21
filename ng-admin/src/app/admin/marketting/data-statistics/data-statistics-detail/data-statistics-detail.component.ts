import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ExhibitionShopServiceProxy, PagedResultDtoOfExhibitionShop } from '@shared/service-proxies/marketing-service';
import { Parameter, ShopReportData } from '@shared/service-proxies/entity';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { AppConsts } from '@shared/AppConsts';
import { ShopReportDataServiceProxy, PagedResultDtoOfShopReportData } from '@shared/service-proxies/marketing-service/shopReportData-service';

@Component({
    moduleId: module.id,
    selector: 'data-statistics-detail',
    templateUrl: 'data-statistics-detail.component.html',
    styleUrls: ['data-statistics-detail.component.scss']
})
export class DataStatisticsDetailComponent extends AppComponentBase implements OnInit {
    organization: string = this.actRouter.snapshot.params['organization'];
    organizatType: number;
    dataStatisticsDetail: ShopReportData[] = [];
    exportLoading = false;
    search: any = {};
    loading = false;

    constructor(injector: Injector, private router: Router, private modal: NzModalService,
        private shopReportDataService: ShopReportDataServiceProxy, private actRouter: ActivatedRoute) {
        super(injector);
        this.organization = this.actRouter.snapshot.params['organization'];
        this.organizatType = this.actRouter.snapshot.params['groupNum'];
    }

    ngOnInit(): void {

        this.refreshData();
    }

    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'Organization', value: this.organization }));
        arry.push(Parameter.fromJS({ key: 'OrganizatType', value: this.organizatType }));
        arry.push(Parameter.fromJS({ key: 'Specification', value: this.search.specification }));
        return arry;
    }
    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = {};
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.shopReportDataService.getShopReportDataDetailAsync(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfShopReportData) => {
            this.loading = false;
            this.dataStatisticsDetail = result.items;
            this.query.total = result.totalCount;
        })
    }

    exportExcel() {
        this.exportLoading = true;
        this.shopReportDataService.exportShopReportDataByOrganizationExcel(
            { organization: this.organization, specification: this.search.specification, organizatType: this.organizatType }).subscribe(result => {
                console.log(this.organizatType, this.search.specification, );

                if (result.code == 0) {
                    var url = AppConsts.remoteServiceBaseUrl + result.data;
                    document.getElementById('aExcelUrl').setAttribute('href', url);
                    document.getElementById('btnHref').click();
                } else {
                    this.notify.error(result.msg);
                }
                this.exportLoading = false;
            });
    }

    return() {
        this.router.navigate(['admin/marketting/data-statistics']);
    }
}
