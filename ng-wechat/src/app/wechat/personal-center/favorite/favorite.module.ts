// Angular Imports
import { NgModule } from '@angular/core';

// This Module's Components
import { SharedModule } from '../../../shared/shared.module';
import { AngularSplitModule } from 'angular-split';
import { ComponentsModule } from '../../components/components.module';
import { RouterModule, Routes } from '@angular/router';
import { FavoriteComponent } from './favorite.component';
import { FavoriteService } from '../../../services';


const COMPONENTS = [FavoriteComponent];

const routes: Routes = [
    { path: '', redirectTo: 'favorite' },
    { path: 'favorite', component: FavoriteComponent },
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
    exports: [
        FavoriteComponent,
    ],
    providers: [
        FavoriteService
    ]
})
export class FavoriteModule {

}
