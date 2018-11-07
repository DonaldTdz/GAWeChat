import { Component, OnInit, Injector, Input, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { DialogConfig, DialogService } from 'ngx-weui';
import { AppConsts, ShopService } from '../../../../services';
import { ActivatedRoute } from '@angular/router';
import { Shop } from '../../../../services/model';
import { Observer } from 'rxjs';
import { LoaderService } from 'ngx-weui/utils/loader.service';
import { isAndroid } from 'ngx-weui/utils/browser';
import { ToastService } from "ngx-weui/toast";

@Component({
    selector: 'shop-qrcode',
    templateUrl: 'shop-qrcode.component.html',
    styleUrls: ['shop-qrcode.component.scss']
})
export class ShopQrcodeComponent extends AppComponentBase implements OnInit {
    @Input() config: DialogConfig;
    @Output() close = new EventEmitter<ShopQrcodeComponent>();
    private observer: Observer<any>;
    shop: Shop;
    qrCodeUrl = '';
    _shown = false;
    isExist = false;
    curIsShopKeeper = true;
    constructor(injector: Injector, private shopService: ShopService, private activeRoute: ActivatedRoute,
        private load: LoaderService, private toastServ: ToastService) {
        super(injector);
    }
    ngOnInit(): void {
        this.isExist = false;
    }

    getIsCurShopKeeper() {
        let params: any =
        {
            shopId: this.shop.id,
            openId: this.settingsService.openId,
        };
        this.shopService.getIsCurShopKeeper(params).subscribe(data => {
            if (data == false) {
                this.curIsShopKeeper = false;
            }
            this.getQrCode();
        });
    }

    show(shop: Shop) {
        //if (this.config.skin === 'auto') {
        //    this.config.skin = isAndroid() ? 'android' : 'ios';
        //}
        this.config.skin = 'ios';
        this.shop = shop;
        //console.log(this.shop);
        if (!this.isExist) {
            this.getIsCurShopKeeper();
        }
        this._shown = true;
    }
    generateQRcode(id: string, url: any) {
        let qrShopCode = new QRCode(id, {
            text: url,
            width: 230,
            height: 230,
            correctLevel: QRCode.CorrectLevel.H
        });
        document.getElementById(id).getElementsByTagName('img').item(0).style.width = '100%';
    }

    /**
    * 隐藏
    *
    * @param is_backdrop 是否从背景上点击
    */
    hide(is_backdrop: boolean = false) {
        if (is_backdrop === true && this.config.backdrop === false) return false;

        this._shown = false;
        this.close.emit(this);
    }
    getQrCode() {
        //console.log(this.shop.id)
        this.toastServ.show(null, 10000, null, 'loading');
        this.load.loadScript('assets/libs/qrcode.min.js').then((res) => {
            let url = '';
            if (this.curIsShopKeeper == true) {
                url = AppConsts.remoteServiceBaseUrl + '/GAWX/ShopAuth?state=' + this.shop.id + '&curIsShopKeeper=' + this.curIsShopKeeper;
            } else {
                let isShowWindows = 'false';
                url = AppConsts.remoteServiceBaseUrl + '/GAWX/ShopAuth?state=' + this.shop.id + '&isShowWindows=' + isShowWindows + '&curIsShopKeeper=' + this.curIsShopKeeper;
            }
            //this.shopService.GetQrCodeUrl({shopId:this.shop.id,host:AppConsts.remoteServiceBaseUrl}).subscribe(data => {
            //生成微信二维码
            this.generateQRcode('wechat_qrcode', url);
            this.isExist = true;
            this.toastServ.hide();
            //});
        })
    }
}
