import { NgModule } from '@angular/core';
import { RetailerManageComponent } from './retailer-manage.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../../../shared/shared.module';
import { AngularSplitModule } from 'angular-split';
import { ComponentsModule } from '../../components/components.module';
import { WechatUserService, DemandForecastService } from '../../../services';
import { RetailerDemandComponent } from './retailer-demand/retailer-demand.component';

const COMPONENTS = [
    RetailerManageComponent
    , RetailerDemandComponent];

const routes: Routes = [
    { path: '', redirectTo: 'emp-retailer' },
    { path: 'emp-retailer', component: RetailerManageComponent },
    { path: 'retailer-demand', component: RetailerDemandComponent },
];
@NgModule({
    imports: [
        SharedModule,
        AngularSplitModule,
        ComponentsModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        ...COMPONENTS
    ],
    providers: [
        WechatUserService,
        DemandForecastService
    ]
})
export class RetailerManageModule {

}
