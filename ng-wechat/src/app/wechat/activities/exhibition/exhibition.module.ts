import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { SharedModule } from '../../../shared/shared.module';
import { ComponentsModule } from '../../components/components.module';
import { ExhibitionComponent } from './exhibition.component';
import { ArticleService } from '../../../services';
import { ExhibitionDetailComponent } from './exhibition-detail/exhibition-detail.component';

const COMPONENTS = [ExhibitionComponent, ExhibitionDetailComponent];

const routes: Routes = [
    { path: 'exhibition', component: ExhibitionComponent },
    { path: 'exhibition-detail', component: ExhibitionDetailComponent }
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
        ArticleService
    ]
})
export class ExhibitionModule {

}
