import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { SharedModule } from '../../../shared/shared.module';
import { ComponentsModule } from '../../components/components.module';
import { ShopComponent } from './shop.component';
import { WaitAuditComponent } from './wait-audit/wait-audit.component';

import { ShopService, FavoriteService } from '../../../services';
import { ShopQrcodeComponent } from './shop-qrcode/shop-qrcode.component';



// region: components

const COMPONENTS = [ShopComponent,
    WaitAuditComponent,
    ShopQrcodeComponent];

const routes: Routes = [
    { path: 'shop', component: ShopComponent },
    { path: 'wait-audit', component: WaitAuditComponent }
];
// endregion

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
        ShopService,
        FavoriteService
    ]
})
export class ShopModule {

}
