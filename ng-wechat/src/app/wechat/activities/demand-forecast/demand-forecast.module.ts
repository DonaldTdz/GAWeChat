import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { SharedModule } from '../../../shared/shared.module';
import { ComponentsModule } from '../../components/components.module';
import { DemandForecastComponent } from './demand-forecast.component';
import { DemandDetailComponent } from './demand-detail/demand-detail.component';
import { DemandForecastService } from '../../../services';

const COMPONENTS = [DemandForecastComponent, DemandDetailComponent];

const routes: Routes = [
    { path: 'demand-forecast', component: DemandForecastComponent },
    { path: 'demand-detail', component: DemandDetailComponent },
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
        DemandForecastService
    ]
})
export class DemandForecastModule { }