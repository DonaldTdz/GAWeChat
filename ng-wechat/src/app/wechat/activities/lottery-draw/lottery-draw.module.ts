import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { SharedModule } from '../../../shared/shared.module';
import { ComponentsModule } from '../../components/components.module';
import { LotteryListComponent } from './lottery-list/lottery-list.component';
import { LotteryDetailComponent } from './lottery-detail/lottery-detail.component';
import { LotteryJoinComponent } from './lottery-join/lottery-join.component';
import { LotterySignInComponent } from './lottery-sign-in/lottery-sign-in.component';
import { LotterySignInListComponent } from './lottery-sign-in-list/lottery-sign-in-list.component';
import { LotterySignInDetailComponent } from './lottery-sign-in-detail/lottery-sign-in-detail.component';
import { LotteryPriceAddComponent } from './lottery-price-add/lottery-price-add.component';
import { LotteryDrawComponent } from './lottery-draw.component';

const COMPONENTS = [        LotterySignInDetailComponent,
    LotterySignInComponent,
    LotterySignInListComponent,
    LotteryJoinComponent,
    LotteryDetailComponent,
    LotteryListComponent,
    LotteryPriceAddComponent,
    LotteryDrawComponent,];

const routes: Routes = [
    { path: '', redirectTo: 'lottery-draw' },
    { path: 'lottery-price-add', component: LotteryPriceAddComponent },
    { path: 'lottery-draw', component: LotteryDrawComponent }
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
    exports: [

    ],
    providers: [
    ]
})
export class LotteryDrawModule { }