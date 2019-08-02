import { NgModule } from '@angular/core';

import { QuestionnaireComponent } from './questionnaire.component';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { ComponentsModule } from '../../components/components.module';
import { SharedModule } from '../../../shared/shared.module';
import { QuestionnaireService, DemandForecastService } from '../../../services';
import { QuestionnaireDetailComponent } from './questionnaire-detail/questionnaire-detail.component';
import { SubmitSuccessComponent } from './submit-success/submit-success.component';
const COMPONENTS = [QuestionnaireComponent, QuestionnaireDetailComponent, SubmitSuccessComponent];

const routes: Routes = [
    { path: 'questionnaire', component: QuestionnaireComponent },
    { path: 'questionnaire-detail', component: QuestionnaireDetailComponent },
    { path: 'submit-success', component: SubmitSuccessComponent }
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
        QuestionnaireService,
        DemandForecastService
    ]
})
export class QuestionnaireModule {

}
