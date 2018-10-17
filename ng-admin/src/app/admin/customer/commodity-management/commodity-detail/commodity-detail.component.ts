import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ProductsServiceProxy } from '@shared/service-proxies/customer-service';
import { ActivatedRoute, Router } from '@angular/router';
import { Products } from '@shared/entity/customer';
import { UploadFile } from 'ng-zorro-antd';
import { AppConsts } from '@shared/AppConsts';
import { HttpRequest, HttpClient, HttpResponse } from '@angular/common/http';
import { filter } from 'rxjs/operators';
import { CropperSettings, ImageCropperComponent, Bounds } from 'ng2-img-cropper';
import { ThemesService } from '@delon/theme';

@Component({
    moduleId: module.id,
    selector: 'commodity-detail',
    templateUrl: 'commodity-detail.component.html',
    styleUrls: ['commodity-detail.component.scss']
})
export class CommodityDetailComponent extends AppComponentBase implements OnInit {
    @ViewChild('cropper', undefined) cropper: ImageCropperComponent;

    form: FormGroup;
    id: number;
    product: Products = new Products();
    isConfirmLoading = false;

    types = [
        { text: '卷烟类', value: 1 },
        { text: '特产类', value: 2 },
    ]
    isRares = [
        { text: '是', value: true },
        { text: '否', value: false },
    ];
    imgTypes = [
        { text: '原图', value: 1 },
        { text: '裁剪', value: 2 },
    ]
    //图片放大
    previewImage = '';
    previewVisible = false;

    host = '';
    photo = '';
    actionUrl = '';
    cardTitle = '';

    defalutImg = '/assets/img/default.png';
    //图片裁剪
    fileList: UploadFile[] = [];
    uploading = false;

    cropperSettings: CropperSettings;
    data1: any = {};
    //是否是上传状态 裁剪
    isUpload = false;
    //是否选中 裁剪
    isSelect = false;

    //是否是上传状态 原图
    isUploadY = false;
    imgType = 1;
    tagsList: string[] = [];
    tagsText: string;
    constructor(injector: Injector, private fb: FormBuilder, private productService: ProductsServiceProxy, private actRouter: ActivatedRoute,
        private router: Router, private http: HttpClient) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
        //图片裁剪数据
        this.cropperSettings = new CropperSettings();
        this.cropperSettings.croppedWidth = 200;
        this.cropperSettings.croppedHeight = 200;
        // this.cropperSettings.width = 200;
        // this.cropperSettings.height = 300;
        this.cropperSettings.cropperDrawSettings.strokeColor = 'rgba(255,255,255,1)';
        this.cropperSettings.cropperDrawSettings.strokeWidth = 2;
        this.cropperSettings.noFileInput = true;
    }

    ngOnInit(): void {
        this.form = this.fb.group({
            specification: [null, Validators.compose([Validators.required, Validators.maxLength(200)])],
            type: [null],
            price: [null],
            isRare: [null],
            packageCode: [null, Validators.compose([Validators.pattern('^[0-9]*$'), this.confirmationValidatorP])],
            barCode: [null, Validators.compose([Validators.pattern('^[0-9]*$'), this.confirmationValidatorB])],
            isAction: [true],
            photoUrl: [null, Validators.compose([Validators.maxLength(500)])],
            imgType: [null],
            desc: [null, Validators.compose([Validators.maxLength(500)])],
            tags: null
        });
        this.getSingleProdct();
        this.host = AppConsts.remoteServiceBaseUrl;
        this.actionUrl = this.host + '/WeChatFile/MarketingInfoPosts?fileName=product';
    }

    confirmationValidatorB = (control: FormControl): { [s: string]: boolean } => {
        if (!control.value) {
            // return { required: true };
        } else if (control.value == this.form.controls['packageCode'].value) {
            return { confirm: true, error: true };
        }
    }
    confirmationValidatorP = (control: FormControl): { [s: string]: boolean } => {
        if (!control.value) {
            // return { required: true };
        } else if (control.value == this.form.controls['barCode'].value) {
            return { confirm: true, error: true };
        }
    }


    getSingleProdct() {
        this.productService.get(this.id).subscribe((result: Products) => {
            this.product = result;
            if (!this.product.id) {
                this.product.init({ isAction: true, photoUrl: this.defalutImg });
                this.cardTitle = '新增商品';
            }
            else {
                this.cardTitle = '编辑商品';
                if (this.product.tags != null && this.product.tags.length != 0) {
                    this.tagsList = this.product.tags.split(',');
                }
                // this.actionUrl=this.host + '/WeChatFile/MarketingInfoPosts?fileName=product&name='+this.product.id;
            }
        });
    }

    getFormControl(name: string) {
        return this.form.controls[name];
    }

    save() {
        //检查form验证
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.isConfirmLoading = true;
            this.productService.CheckCode(this.product.id, this.product.packageCode, this.product.barCode).subscribe((result: number) => {
                if (result === 0) {
                    this.product.tags = this.tagsList.join(',');
                    this.productService.update(this.product)
                        .finally(() => { this.isConfirmLoading = false; })
                        .subscribe((data) => {
                            this.product = data;
                            this.notify.info(this.l('保存成功！'));
                            this.isUpload = false;
                            this.isUploadY = false;
                        });
                } else {
                    this.isConfirmLoading = false;
                    var errorMsg = result === 1 ? '包码重复' : (result === 2 ? '条码重复' : (result === 3 ? '包码、条码重复' : ''));
                    this.notify.error(this.l(errorMsg));
                }
            });
        }
    }
    /**
     * 返回
     */
    Return() {
        this.router.navigate(['admin/customer/commodity-management']);
    }


    //图片放大
    handlePreview = (url: string) => {
        this.previewImage = url;
        this.previewVisible = true;
    }
    //#region 前图片上传

    // private getBase64(img: File, callback: (img: any) => void) {
    //     const reader = new FileReader();
    //     reader.addEventListener('load', () => callback(reader.result));
    //     reader.readAsDataURL(img);
    // }

    // handleChange(info: { file: UploadFile }): void {
    //     console.table(info);

    //     if (info.file.status === 'error') {
    //         this.notify.error('上传图片异常，请重试');
    //     }
    //     if (info.file.status === 'done') {
    //         this.getBase64(info.file.originFileObj, (img: any) => {
    //             // this.loading = false;
    //             // this.photo = img;
    //             this.product.showPhotoUrl = img;
    //         });
    //         this.product.photoUrl = info.file.response.result.imageName;
    //         console.log('photoUrl')
    //         console.log(this.product.photoUrl);
    //         console.log('imageName')
    //         console.log(info.file.response.result.imageName);
    //         this.notify.success('上传图片完成');
    //     }
    // }

    //endregion

    //剪切结果返回
    cropped(bounds: Bounds) {
        // console.log(bounds);
        //   console.log(this.data1.image);
        this.product.img64 = this.data1.image;

    }

    //选择图片文件
    fileChange($event) {

        this.isSelect = true;
        const image: any = new Image();
        const file: File = $event.target.files[0];
        const myReader: FileReader = new FileReader();
        const that = this;
        myReader.onloadend = (loadEvent: any) => {
            image.src = loadEvent.target.result;
            this.product.fileName = file.name;
            if (this.imgType === 1) {
                this.product.showPhotoUrl = image.src;
                this.product.img64 = image.src;
                this.isUploadY = true;
            } else {
                this.isUpload = true;
                setTimeout(() => {
                    that.cropper.setImage(image);
                }, 200);

            }

            // this.selecteCutImgModal.show(image);
        };
        myReader.readAsDataURL(file);
    }
    //剪切确定
    // Select(){
    //     this.isSelect=false;
    //     this.product.img64=this.data1.image;
    // }

    addTag(tagsText: string) {
        if (tagsText != null && tagsText.trim().length !== 0 && !this.existsEmployee(tagsText)) {
            this.tagsText = this.tagsText.replace(/ /g, '').replace(/[\ |\~|\`|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\-|\_|\+|\=|\||\\|\[|\]|\{|\}|\;|\:|\"|\'|\,|\<|\.|\>|\/|\?]/g, "")
            this.tagsList.push(this.tagsText);
        }
        this.tagsText = null;
    }

    existsEmployee(tagsText: string): boolean {
        let bo = false;
        this.tagsList.forEach(v => {
            if (v == tagsText) {
                bo = true;
                return;
            }
        });
        return bo;
    }

    onClose(event: Event, tagsText: string): void {
        let i = 0;
        this.tagsList.forEach(v => {
            if (v == tagsText) {
                this.tagsList.splice(i, 1);
                return;
            }
            i++;
        });
        if (this.tagsList.length < 0) {
            this.tagsList = null;
        }
    }
}
