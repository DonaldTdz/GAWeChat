import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ProductsServiceProxy, PagedResultDtoOfProducts } from '@shared/service-proxies/customer-service';
import { Products } from '@shared/entity/customer';
import { Parameter } from '@shared/service-proxies/entity';
import { NzModalService } from 'ng-zorro-antd';
import { Router } from '@angular/router';
import { AppConsts } from '@shared/AppConsts';
import { FormGroup, FormBuilder, Validators } from '../../../../../node_modules/@angular/forms';
import { MemberConfigs } from '@shared/entity/member/memberconfig';
import { MemberConfigsServiceProxy, PagedResultDtoOfMemberConfigs } from '@shared/service-proxies/member/memberconfigs-service';

@Component({
    moduleId: module.id,
    selector: 'commodity-management',
    templateUrl: 'commodity-management.component.html',
    styleUrls: ['commodity-management.component.scss']

})
export class CommodityManagementComponent extends AppComponentBase implements OnInit {
    loading = false;
    status = [
        { text: '启用', value: true, type: 'success' },
        { text: '禁用', value: false, type: 'default' },
    ];
    isRares = [
        { text: '全部', value: 0 },
        { text: '是', value: true },
        { text: '否', value: false },
    ];
    types = [
        { text: '全部', value: 0 },
        { text: '卷烟类', value: 1 },
        { text: '特产类', value: 2 },
    ]
    products: Products[] = [];
    search: any = { isRare: 0, type: 0 }
    //图片放大
    previewImage = ''
    previewVisible = false;
    imgWidth: number = 550;
    defalutImg = '/assets/img/default.png';
    // defalutImg = '/upload/product/tobacco.jpg';
    productName = '';
    sortValue = null;
    host = '';
    exportLoading = false;
    form: FormGroup;
    preConfig: MemberConfigs = new MemberConfigs();

    constructor(injector: Injector, private fb: FormBuilder, private productsService: ProductsServiceProxy,
        private configService: MemberConfigsServiceProxy, private modal: NzModalService,
        private router: Router) {
        super(injector);
    }
    ngOnInit(): void {
        this.form = this.fb.group({
            value: [null, Validators.compose([Validators.required])],
        });
        this.refreshData();
        this.getPreProductConfig();
        this.host = AppConsts.remoteServiceBaseUrl;
        // this.defalutImg = this.host + this.defalutImg;
    }

    getFormControl(id: string) {
        return this.form.controls[id];
    }

    sort(value) {
        this.sortValue = value;
        this.refreshData();
    }

    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = { isRare: 0, type: 0 };
            this.sortValue = null;
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.productsService.getAll(this.query.skipCount(), this.query.pageSize, this.getParameter()).subscribe((result: PagedResultDtoOfProducts) => {
            this.loading = false;
            let status = 0;
            this.products = result.items.map(i => {
                // if(i.photoUrl){
                //     i.photoUrl = i.photoUrl;
                // }else{
                //     i.photoUrl= this.defalutImg;
                // }
                if (i.isAction) {
                    status = 0;
                } else {
                    status = 1;
                }
                const statusItem = this.status[status];
                i.activeText = statusItem.text;
                i.activeType = statusItem.type;
                return i;
            })
            this.query.total = result.totalCount;
        })

    }
    getParameter(): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'Name', value: this.search.name }));
        arry.push(Parameter.fromJS({ key: 'Type', value: this.search.type === 0 ? null : this.search.type }));
        arry.push(Parameter.fromJS({ key: 'IsRare', value: this.search.isRare === 0 ? null : this.search.isRare }));
        arry.push(Parameter.fromJS({ key: 'SortValue', value: this.sortValue }));
        return arry;
    }
    handlePreview = (url: string) => {
        if (!url) {
            this.previewImage = this.defalutImg;
        }
        else {
            this.previewImage = url;
        }
        this.previewVisible = true;
    }

    /**
     * 删除单个商品
     * @param product 商品实体
     * @param contentTpl 弹框id
     */
    delete(product: Products, contentTpl): void {
        this.productName = product.specification;
        this.modal.confirm({
            content: contentTpl,
            okText: '是',
            cancelText: '否',
            onOk: () => {
                this.productsService.delete(product.id).subscribe(() => {
                    this.notify.info(this.l('删除成功！'));
                    this.refreshData();
                })
            }
        });
    }

    editProduct(product: Products) {
        this.router.navigate(['admin/customer/commodity-detail', product.id])
    }

    createProduct() {
        this.router.navigate(['admin/customer/commodity-detail'])

    }

    /**
     * 导出商品信息
     */
    exportExcel() {
        this.exportLoading = true;
        this.productsService.ExportExcel({ name: this.search.name, type: this.search.type === 0 ? null : this.search.type, isRare: this.search.isRare === 0 ? null : this.search.isRare, sortValue: this.sortValue }).subscribe(data => {
            if (data.code == 0) {
                var url = AppConsts.remoteServiceBaseUrl + data.data;
                document.getElementById('aProductsExcelUrl').setAttribute('href', url);
                document.getElementById('btnProductsHref').click();
            } else {
                this.notify.error(data.msg);
            }
            this.exportLoading = false;
        });
    }

    getPreProductConfig() {
        this.configService.getPreProductConfigAsync().subscribe((result: MemberConfigs) => {
            this.preConfig = result;
        });
    }

    save() {
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.loading = true;
            console.log(this.preConfig);
            this.configService.updatePreProduct(this.preConfig).subscribe(() => {
                this.notify.info('保存成功！');
                this.getPreProductConfig();
                this.loading = false;
            });
        }
    }
}
