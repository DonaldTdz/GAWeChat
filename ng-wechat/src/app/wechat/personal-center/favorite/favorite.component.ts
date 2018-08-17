import { Component, ViewEncapsulation, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { FavoriteService, AppConsts } from '../../../services';
import { Router } from '../../../../../node_modules/@angular/router';
import { Favorite } from '../../../services/model';

@Component({
    moduleId: module.id,
    selector: 'favorite',
    templateUrl: 'favorite.component.html',
    styleUrls: ['favorite.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class FavoriteComponent extends AppComponentBase implements OnInit {
    favoriteList: Favorite[] = [];
    hostUrl: string = AppConsts.remoteServiceBaseUrl;

    constructor(injector: Injector, private favoriteService: FavoriteService, private router: Router) {
        super(injector);
    }
    ngOnInit() {
        this.GetMyFavoriteShopsAsync();
        console.log(this.hostUrl);

    }

    GetMyFavoriteShopsAsync() {
        let params: any =
        {
            openId: this.settingsService.openId,
        };
        this.favoriteService.GetWXMyFavoriteShopsAsync(params).subscribe(data => {
            this.favoriteList.push(...data);
        });
    }
    goFindGoods() {
        this.router.navigate(['/nearbies/nearby']);
    }
    goDetailShop(id: string) {
        this.router.navigate(['/shops/shop', { shopId: id }]);
    }
}

