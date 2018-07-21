import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ExhibitionShopServiceProxy, PagedResultDtoOfExhibitionShop } from '@shared/service-proxies/marketing-service';
import { Parameter } from '@shared/service-proxies/entity';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';
import { ExhibitionShop } from '@shared/entity/marketting';
import { AppConsts } from '@shared/AppConsts';

@Component({
    moduleId: module.id,
    selector: 'exhibition-detail',
    templateUrl: 'exhibition-detail.component.html',
    styleUrls: ['exhibition-detail.component.scss']
})
export class ExhibitionDetailComponent extends AppComponentBase implements OnInit {
    exhibitionShop: ExhibitionShop = new ExhibitionShop;
    id: string;
    shopName = '';
    picIds: string[] = [];
    previewImage = '';
    previewVisible = false;
    //图片路径前缀
    host = '';

    constructor(injector: Injector,
        private router: Router, private modal: NzModalService, private actRouter: ActivatedRoute,
        private exhibitionShopService: ExhibitionShopServiceProxy, ) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }
    ngOnInit(): void {
        this.getExhibitionDetail();
        this.host = AppConsts.remoteServiceBaseUrl;
    }
    getExhibitionDetail() {
        this.exhibitionShopService.getExhibitionDetailById(this.id).subscribe((result: ExhibitionShop) => {
            this.exhibitionShop = result;
            // if (result.picPath.indexOf(',') != -1) {
            if (result.picPath != '') {
                this.picIds = this.exhibitionShop.picPath.split(',');
            }
        })
    }

    //图片放大
    handlePreview = (url: string) => {
        this.previewImage = url;
        this.previewVisible = true;
    }

    return() {
        this.router.navigate(['admin/marketting/exhibition']);
    }
}
