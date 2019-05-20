import { Component, Input, ViewEncapsulation } from '@angular/core';

@Component({
    selector: 'Page',
    template: `
    <div class="page__hd" *ngIf="showTitle" >
        <h1 class="page__title" [innerHTML]="title"></h1>
        <p class="page__desc" [innerHTML]="subTitle"></p>
    </div>
    <div class="weui-cells" *ngIf="showBack" style="margin-top:0px;">
    <div class="weui-cell">
        <div class="weui-cell__hd">
        <i class="iconfont icon-left"></i>
        </div>
        <div class="weui-cell__bd">
        <a [routerLink]="backUrl" style="color:#353535;text-decoration:none; " > {{title}}</a>
        </div>
    </div>
    </div>
    <div class="page__bd" [ngClass]="{'page__bd_spacing': spacing}"><ng-content></ng-content></div>
    <div class="page__ft" *ngIf="!noBottom">
    <div class="weui-footer">
        <p class="weui-footer__links" *ngIf="showBindStaff">
        <a [routerLink]="['/personals/bind-staff']" class="weui-footer__link">员工绑定</a>
        </p>
        <p class="weui-footer__text">XXXX | XXXX</p>
    </div>
        <ng-content select="[footer]"></ng-content>
    </div>
    `,
    host: {
        'class': 'page'
    },
    styleUrls: ['./page.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class PageComponent {
    @Input() title: string;
    @Input() subTitle: string;
    @Input() spacing: boolean = true;
    @Input() ftBottom: boolean = false;
    @Input() noBottom: boolean = false;
    @Input() showTitle: boolean = true;
    @Input() showBindStaff: boolean = false;
    @Input() showBack: boolean = false;
    @Input() backUrl: string;
}
