import { NgModule } from "@angular/core";
import { AppRouteGuard } from "@shared/auth/auth-route-guard";
import { SharedModule } from "@shared/shared.module";
import { EmployeesComponent } from "./employees/employees.component";
import { CreateEmployeeComponent } from "./employees/create-employee/create-employee.component";
import { EditEmployeeComponent } from "./employees/edit-employee/edit-employee.component";
import { ActivityComponent } from "./activity/activity.component";
import { ExperienceShareComponent } from "./experience-share/experience-share.component";
import { ContributeManagementComponent } from "./contribute-management/contribute-management.component";
import { MarkettingRoutingModule } from "./marketting-routing.module";
import { LayoutModule } from "../../layout/layout.module";
import { ActivityDetailComponent } from "./activity/activity-detail/activity-detail.component";
import { ExperienceDetailComponent } from "./experience-share/experience-detail/experience-detail.component";
import { ContributeDetailComponent } from "./contribute-management/contribute-detail/contribute-detail.component";
import { NgxTinymceModule } from 'ngx-tinymce';
import { EditorModule } from '@tinymce/tinymce-angular';
import { ExhibitionComponent } from "./exhibition/exhibition.component";
import { ExhibitionDetailComponent } from "./exhibition/exhibition-detail/exhibition-detail.component";
import { DataStatisticsComponent } from "./data-statistics/data-statistics.component";
import { DataStatisticsDetailComponent } from "./data-statistics/data-statistics-detail/data-statistics-detail.component";
import { DemandForecastComponent } from "./demand-forecast/demand-forecast.component";
import { QuestionnaireComponent } from "./questionnaire/questionnaire.component";
import { EditQuestionnaireComponent } from "./questionnaire/edit-questionnaire/edit-questionnaire.component";

@NgModule({
    imports: [
        SharedModule,
        LayoutModule,
        MarkettingRoutingModule,
        EditorModule,// <- Important part
        NgxTinymceModule.forRoot({
            baseURL: './assets/tinymce/',
        })
    ],
    declarations: [
        EmployeesComponent,
        CreateEmployeeComponent,
        EditEmployeeComponent,
        ActivityComponent,
        ActivityDetailComponent,
        ExperienceShareComponent,
        ExperienceDetailComponent,
        ContributeManagementComponent,
        ContributeDetailComponent,
        ExhibitionComponent,
        ExhibitionDetailComponent,
        DataStatisticsComponent,
        DataStatisticsDetailComponent,
        DemandForecastComponent,
        QuestionnaireComponent,
        EditQuestionnaireComponent
    ],
    providers: [
        AppRouteGuard
    ]

})
export class MarkettingModule { }