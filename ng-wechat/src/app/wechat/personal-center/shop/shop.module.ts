import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AngularSplitModule } from 'angular-split';
import { SharedModule } from '../../../shared/shared.module';
import { ComponentsModule } from '../../components/components.module';
import { ShopComponent } from './shop.component';
import { WaitAuditComponent } from './wait-audit/wait-audit.component';

import { ShopService, FavoriteService, WechatUserService } from '../../../services';
import { ShopQrcodeComponent } from './shop-qrcode/shop-qrcode.component';
import { ShopGoodDetailComponent } from './shop-good-detail/shop-good-detail.component';



// region: components

const COMPONENTS = [ShopComponent,
    WaitAuditComponent,
    ShopQrcodeComponent,
    ShopGoodDetailComponent
];

const routes: Routes = [
    { path: 'shop', component: ShopComponent },
    { path: 'wait-audit', component: WaitAuditComponent },
    { path: 'shop-good-detail', component: ShopGoodDetailComponent }
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
        FavoriteService,
        WechatUserService
    ]
})
export class ShopModule {

}
