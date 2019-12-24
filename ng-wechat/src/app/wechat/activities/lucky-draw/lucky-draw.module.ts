import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { SharedModule } from '../../../shared/shared.module';
import { ComponentsModule } from '../../components/components.module';
import { LuckyDrawComponent } from './lucky-draw.component';
import { LotteryService } from '../../../services';

const COMPONENTS = [LuckyDrawComponent];

const routes: Routes = [
    { path: 'lucky-draw', component: LuckyDrawComponent },
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
        LotteryService
    ]
})
export class LuckyDrawModule {
}