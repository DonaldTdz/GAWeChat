import { NgModule } from "@angular/core";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";

import { SharedModule } from "@shared/shared.module";
import { RetailCustomerComponent } from "./retail-customer/retail-customer.component";
import { RetailCustomerDetailComponent } from "./retail-customer/retail-customer-detail/retail-customer-detail.component";
import { StoreManagementComponent } from "./store-management/store-management.component";
import { EmployeeModalComponent } from "./retail-customer/retail-customer-detail/employee-modal/employee-modal.component";
import { CommodityManagementComponent } from "./commodity-management/commodity-management.component";
import { SourceGoodsLevelComponent } from "./source-goods-level/source-goods-level.component";
import { AccountManagementComponent } from "./account-management/account-management.component";
import { LayoutModule } from "../../layout/layout.module";
import { CustomerRoutingModule } from "./customer-routing.module";
import { StoreDetailComponent } from "./store-management/store-detail/store-detail.component";
import { CommodityDetailComponent } from "./commodity-management/commodity-detail/commodity-detail.component";
import { ImageCropperModule } from "ng2-img-cropper";
import { ColorPickerModule } from "ngx-color-picker";
import { RefuseComponent } from "./store-management/store-detail/refuse/refuse.component";
import { CommonModule } from "@angular/common";
import { AlainABCModule } from "@delon/abc";
import { RetailStatisticsComponent } from "./retail-customer/retail-statistics/retail-statistics.component";
import { RetailDemandDetailComponent } from "./retail-customer/retail-statistics/retail-demand-detail/retail-demand-detail.component";
import { RetailAnswerRecordComponent } from "./retail-customer/retail-answer-record/retail-answer-record.component";

@NgModule({
    imports: [
        SharedModule,
        LayoutModule,
        CustomerRoutingModule,
        ImageCropperModule,
        ColorPickerModule,
        CommonModule,
        // AdQRModule ,
        AlainABCModule
    ],
    declarations: [
        RetailCustomerComponent,
        RetailCustomerDetailComponent,
        EmployeeModalComponent,
        StoreManagementComponent,
        StoreDetailComponent,
        CommodityManagementComponent,
        CommodityDetailComponent,
        SourceGoodsLevelComponent,
        AccountManagementComponent,
        RefuseComponent,
        RetailStatisticsComponent,
        RetailDemandDetailComponent,
        RetailAnswerRecordComponent,
    ],
    providers: [
        AppRouteGuard
    ]

})
export class CustomerModule { }