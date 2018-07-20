import { NzMessageService } from 'ng-zorro-antd';
import { Component, OnInit } from '@angular/core';
import { _HttpClient } from '@delon/theme';
import { AppSessionService } from '@shared/session/app-session.service';
import { ShopServiceProxy } from '@shared/service-proxies/customer-service';
import { HomeInfo, Parameter, ShopStatistic, WeChatUserStatistic } from '@shared/service-proxies/entity';
import { SettingsService } from '@delon/theme';

import { UploadFile } from 'ng-zorro-antd';
import { AdviseService, PagedResultDtoOfAdvise } from '@shared/service-proxies/consumer-service/advise-service';
import { Advise } from '@shared/entity/consumer';
import { Router } from '@angular/router';
import { WechatUserServiceProxy } from '@shared/service-proxies/wechat-service';

@Component({
    selector: 'app-home-index',
    templateUrl: './index.component.html',
    styleUrls: ['index.component.scss']
})
export class IndexComponent implements OnInit {
    adviseList: Advise[] = [];
    homeInfo: HomeInfo = new HomeInfo();
    shopStatistics: ShopStatistic[] = [];
    wechatUserStatistic: WeChatUserStatistic[] = [];
    constructor(private _adviseService: AdviseService, private http: _HttpClient, public msg: NzMessageService, private _appSessionService: AppSessionService,
        private shopServiceProxy: ShopServiceProxy, private settings: SettingsService, private _router: Router,
        private wechatUserService: WechatUserServiceProxy) { }

    quickMenu = false;

    webSite: any[] = [];
    salesData: any[] = [];
    offlineChartData: any[] = [];
    roleName: string = '';

    previewImage = '';
    previewVisible = false;

    fileList = [];
    shopData: any[] = [];
    wechatData: any[] = [];
    stotal: number;
    wtotal: number;
    ngOnInit() {
        //近12个月申请单数 (模拟数据)
        const sd = [];
        for (let i = 0; i < 12; i += 1) {
            sd.push({
                x: `${i + 1}月`,
                y: Math.floor(Math.random() * 1000) + 200
            });
        }

        this.salesData = sd;
        //24小时网站访问量和活动申请单笔数 (模拟数据)
        const od = [];
        for (let i = 0; i < 20; i += 1) {
            od.push({
                x: new Date().getTime() + 1000 * 60 * 30 * i,
                y1: Math.floor(Math.random() * 100) + 10,
                y2: Math.floor(Math.random() * 100) + 10
            });
        }

        this.offlineChartData = od;


        let roles = this._appSessionService.roles;
        if (roles.includes('HostAdmin')) {
            this.roleName += '系统管理员';
        }
        if (roles.includes('Admin')) {
            this.roleName += '管理员';
        }

        if (roles.includes('MarketingCenter')) {
            this.roleName += ' 营销中心';
        }

        if (roles.includes('CustomerManager')) {
            this.roleName += ' 客户经理';
        }
        this.getFormInfo();
        this.getAdvise();
        this.getShopStatistic();
        this.getWechatUserStatistic();
    }

    getFormInfo() {
        this.shopServiceProxy.getHomeInfo().subscribe((result) => {
            this.homeInfo = result;
        })
    }

    handlePreview = (file: UploadFile) => {
        this.previewImage = file.url || file.thumbUrl;
        this.previewVisible = true;
    }

    /**
     * 获取最新五条意见反馈
     */
    getAdvise() {
        this._adviseService.getAll(0, 5, this.getParameter()).subscribe((result: PagedResultDtoOfAdvise) => {
            this.adviseList = result.items;
        })
    }
    getParameter(): Parameter[] {
        let parray = [];
        parray.push(Parameter.fromJS({}));
        return parray;
    }

    /**
     * 反馈详情
     * @param advise 
     */
    adviseDetail(advise: Advise) {
        this._router.navigate(['admin/consumer/advise-detail', advise.id]);
    }

    /**
     * 获取店铺统计信息
     */
    getShopStatistic() {
        this.shopServiceProxy.getShopStatistic().subscribe(data => {
            console.log(data);
            this.shopStatistics = data.items.map(i => {
                // var count = i.company.indexOf('烟草分公司');
                // i.showCompany =i.company=='其它' ?i.company:i.company.substring(0, count);
                if (i.company != null) {
                    i.showCompany = i.company.replace('烟草分公司', '');
                }
                return i
            });
            this.stotal = data.total;
            console.table(this.shopStatistics);
            const sd = [];
            for (let i = 0; i < this.shopStatistics.length; i += 1) {
                sd.push({
                    x: this.shopStatistics[i].showCompany,
                    y: this.shopStatistics[i].count
                });
            }
            this.shopData = sd;
        });
    }

    /**
     * 获取关注微信用户统计
     */
    getWechatUserStatistic() {
        this.wechatUserService.getWeChatUserStatistic().subscribe(data => {
            console.table(data);
            this.wechatUserStatistic = data.items.map(i => {
                //var count = i.company.indexOf('烟草分公司');
                if (i.company != null) {
                    i.showCompany = i.company.replace('烟草分公司', '');
                }
                return i
            });
            this.wtotal = data.total;
            const sd = [];
            for (let i = 0; i < this.wechatUserStatistic.length; i += 1) {
                sd.push({
                    x: this.wechatUserStatistic[i].showCompany,
                    y: this.wechatUserStatistic[i].count
                });
            }
            this.wechatData = sd;
        });
    }
}
