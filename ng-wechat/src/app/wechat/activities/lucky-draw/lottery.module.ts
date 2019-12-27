import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { SharedModule } from '../../../shared/shared.module';
import { ComponentsModule } from '../../components/components.module';
import { LotteryService, SettingsService } from '../../../services';
import { LotterySignInDetailComponent } from './lottery-sign-in-detail/lottery-sign-in-detail.component';
import { LotterySignInComponent } from './lottery-sign-in/lottery-sign-in.component';
import { LotterySignInListComponent } from './lottery-sign-in-list/lottery-sign-in-list.component';
import { LotteryDetailComponent } from './lottery-detail/lottery-detail.component';
import { LotteryListComponent } from './lottery-list/lottery-list.component';
import { LotteryPriceAddComponent } from './lottery-price-add/lottery-price-add.component';
import { LotteryActivitiesListComponent } from './lottery-activities-list/lottery-activities-list.component';
import { LotteryComponent } from './lottery.component';
import { LotteryActivityDetailComponent } from './lottery-activity-detail/lottery-activity-detail.component';

const COMPONENTS = [LotterySignInDetailComponent,
    LotterySignInComponent,
    LotterySignInListComponent,
    LotteryDetailComponent,
    LotteryListComponent,
    LotteryPriceAddComponent,
    LotteryComponent,
    LotteryActivitiesListComponent,
    LotteryComponent,
    LotteryActivitiesListComponent,
    LotteryActivityDetailComponent
];

const routes: Routes = [
    { path: '', redirectTo: 'lottery' },
    { path: 'lottery', component: LotteryComponent },
    { path: 'lottery-price-add', component: LotteryPriceAddComponent },
    { path: 'lottery-list', component: LotteryListComponent },
    { path: 'lottery-activities-list', component: LotteryActivitiesListComponent },
    { path: 'lottery-sign-in-list', component: LotterySignInListComponent },
    { path: 'lottery-sign-in', component: LotterySignInComponent },
    { path: 'lottery-sign-in-detail', component: LotterySignInDetailComponent },
    { path: 'lottery-detail', component: LotteryDetailComponent },
    { path: 'lottery-activity-detail', component: LotteryActivityDetailComponent }
];
@NgModule({
    imports: [
        SharedModule,
        AngularSplitModule,
        ComponentsModule,
        RouterModule.forChild(routes)
    ],
    declarations: [
        ...COMPONENTS,
    ],
    providers: [
        LotteryService,
    ]
})
export class LotteryModule {
}