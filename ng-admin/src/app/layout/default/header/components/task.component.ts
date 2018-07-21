import { Component, OnInit } from '@angular/core';
import { SettingsService } from '@delon/theme';
import { Shop } from '@shared/entity/customer/shop';
import { ShopServiceProxy } from '@shared/service-proxies/customer-service';
import { Router } from '@angular/router';

@Component({
    selector: 'header-task',
    template: `
    <nz-dropdown nzTrigger="click" nzPlacement="bottomRight" (nzVisibleChange)="change()">
        <div class="item" nz-dropdown>
            <nz-badge [nzDot]="false" [nzCount]="count">
                <ng-template #content>
                    <i class="anticon anticon-bell"></i>
                </ng-template>
            </nz-badge>
        </div>
        <div nz-menu class="wd-lg" style="width:500px;">
            <nz-card nzTitle="待审核店铺" [nzLoading]="loading" class="ant-card__body-nopadding">
                <ng-template #extra></ng-template>
                <div nz-row [nzType]="'flex'" *ngFor="let item of data" (click)="goShop(item.id)" [nzJustify]="'center'" [nzAlign]="'middle'" class="py-sm bg-grey-lighter-h point">
                    <div nz-col [nzSpan]="22">
                        <div>
                        <span style="width:120px; float:right; text-algin:right;">{{item.creationTime | _date}}</span>
                        <strong>{{item.name}}</strong>
                        </div>
                        <p>{{item.address}} {{item.tel}}</p>
                       
                    </div>
                </div>
                <div nz-row [nzType]="'flex'" *ngIf="data.length == 0" [nzJustify]="'center'" [nzAlign]="'middle'" class="py-sm bg-grey-lighter-h point">
                <div nz-col [nzSpan]="22" style="text-align:center">
                    <p>没有需要审核的店铺</p>
                </div>
                </div>
                <div nz-row class="pt-lg pb-lg" *ngIf="count > 5">
                    <div nz-col [nzSpan]="24" class="text-center text-grey point" (click)="goShopList()" >
                        查看更多
                    </div>
                </div>
            </nz-card>
        </div>
    </nz-dropdown>
    `
})
export class HeaderTaskComponent implements OnInit {

    loading = true;
    count: number;
    data = [];

    constructor(private settings: SettingsService, private shopServer: ShopServiceProxy, private router: Router) { }

    ngOnInit() {
        // mock data
        this.count = this.settings.user.notifyCount;
    }

    change() {
        this.shopServer.getPendingShopList().subscribe((res) => {
            this.loading = false;
            if (res) {
                this.data = res.data.shopList;
                this.count = res.data.count;
            } else {
                this.count = null;
            }
            this.settings.user.notifyCount = this.count;
        });
    }

    goShop(id) {
        this.router.navigate(['admin/customer/store-detail', id]);
    }

    goShopList() {
        this.router.navigate(['admin/customer/store-management']);
    }

    changeCount(){
        
    }

}
