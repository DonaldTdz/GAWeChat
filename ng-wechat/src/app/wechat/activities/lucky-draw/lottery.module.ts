import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { SharedModule } from '../../../shared/shared.module';
import { ComponentsModule } from '../../components/components.module';
import { LotteryService } from '../../../services';
import { LotterySignInDetailComponent } from './lottery-sign-in-detail/lottery-sign-in-detail.component';
import { LotterySignInComponent } from './lottery-sign-in/lottery-sign-in.component';
import { LotterySignInListComponent } from './lottery-sign-in-list/lottery-sign-in-list.component';
import { LotteryDetailComponent } from './lottery-detail/lottery-detail.component';
import { LotteryListComponent } from './lottery-list/lottery-list.component';
import { LotteryDrawComponent } from './lottery-draw.component';
import { LotteryActivitiesListComponent } from './lottery-activities-list/lottery-activities-list.component';
import { LotteryComponent } from './lottery.component';
import { LotteryActivityDetailComponent } from './lottery-activity-detail/lottery-activity-detail.component';
import { LotteryActivityJoinListComponent } from './lottery-activity-join-list/lottery-activity-join-list.component';
import { LotteryActivityJoinDetailComponent } from './lottery-activity-join-detail/lottery-activity-join-detail.component';
import { LotteryActivityEditComponent } from './lottery-activity-edit/lottery-activity-edit.component';

const COMPONENTS = [LotterySignInDetailComponent,
    LotterySignInComponent,
    LotterySignInListComponent,
    LotteryDetailComponent,
    LotteryListComponent,
    LotteryDrawComponent,
    LotteryActivitiesListComponent,
    LotteryActivityDetailComponent,
    LotteryActivityJoinListComponent,
    LotteryActivityJoinDetailComponent,

];

const routes: Routes = [
    { path: '', redirectTo: 'lottery-draw' },
    { path: 'lottery-draw', component: LotteryDrawComponent },
    { path: 'lottery-list', component: LotteryListComponent },
    { path: 'lottery-activities-list', component: LotteryActivitiesListComponent },
    { path: 'lottery-sign-in-list', component: LotterySignInListComponent },
    { path: 'lottery-sign-in', component: LotterySignInComponent },
    { path: 'lottery-sign-in-detail', component: LotterySignInDetailComponent },
    { path: 'lottery-detail', component: LotteryDetailComponent },
    { path: 'lottery-activity-detail', component: LotteryActivityDetailComponent },
    { path: 'lottery-activity-join-list', component:LotteryActivityJoinListComponent},
    { path: 'lottery-activity-join-detail', component:LotteryActivityJoinDetailComponent},
    { path: 'lottery-activity-edit', component:LotteryActivityEditComponent},
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
        LotteryActivityEditComponent,
    ],
    providers: [
        LotteryService,
    ]
})
export class LotteryModule {
}