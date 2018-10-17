import { Component, Injector, OnInit, ViewEncapsulation } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { Params } from '@angular/router';
import { ShopProduct } from '../../../../../app/services/model';
import { ShopService } from '../../../../services/personal-center/shop.service';
import { AppConsts } from '../../../../services';

@Component({
    moduleId: module.id,
    selector: 'shop-good-detail',
    templateUrl: 'shop-good-detail.component.html',
    styleUrls: ['shop-good-detail.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ShopGoodDetailComponent extends AppComponentBase implements OnInit {

    product: ShopProduct = new ShopProduct();
    hostUrl: string = AppConsts.remoteServiceBaseUrl;
    tagsList: string[] = [];

    constructor(injector: Injector,
        private shopService: ShopService,
    ) {
        super(injector);
    }
    ngOnInit() {
        this.activatedRoute.params.subscribe((params: Params) => {
            this.id = params['id'];
        });
        this.getProduct();
    }
    getProduct() {
        this.shopService.GetProductInfo(this.id).subscribe(res => {
            this.product = res;
            if (this.product.tags != null && this.product.tags.length != 0) {
                this.tagsList = this.product.tags.split(',');
            }
        })
    }
}
