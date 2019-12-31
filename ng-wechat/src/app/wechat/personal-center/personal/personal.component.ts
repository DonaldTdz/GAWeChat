import { Component, ViewEncapsulation, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { WechatUser, UserType } from '../../../services/model';
import { Router } from '@angular/router';
import { PurchaserecordService, WechatUserService } from '../../../services';

@Component({
    selector: 'wechat-personal',
    templateUrl: './personal.component.html',
    styleUrls: ['./personal.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class PersonalComponent extends AppComponentBase implements OnInit {

    user: WechatUser;
    isLotteryAdmin: boolean = true;//是否为抽奖活动管理员
    isShowRetailer: boolean = false;
    accountTitle: string = '我的台账';
    countNotEvaluation: number;
    noCheckShopEmployeeCount: number;
    is
    constructor(injector: Injector, private router: Router, private purchaserecordService: PurchaserecordService,
        private wechatUserService: WechatUserService) {
        super(injector);
    }

    ngOnInit() {
        this.settingsService.getUser().subscribe(result => {
            this.user = result;
            if (this.user.userType == UserType.Retailer && this.user.status == 1) {//零售客户
                this.isShowRetailer = true;
                this.accountTitle = '我的台账';
            } else if (this.user.userType == UserType.Staff) {//内部员工
                this.getIsLotteryAdmin();
                this.isShowRetailer = true;
                this.accountTitle = '台账查询';
            }
            this.getNoCheckShopEmployeeCount();
        });
        // 以.then()方式处理，待确认
        this.GetCountNotEvaluationById();
    }

    /**
     * 展示未评价数目    
     * @memberof PersonalComponent
     */
    GetCountNotEvaluationById() {
        let params: any = {};
        if (this.settingsService.tenantId) {
            params.tenantId = this.settingsService.tenantId;
        }
        params.openId = this.settingsService.openId;
        this.purchaserecordService.GetWXCountNotEvaluationByIdAsync(params).subscribe(result => {
            this.countNotEvaluation = result;
        });
    }

    goIntegralTotal(openId: string) {
        this.router.navigate(["/integrals/integral", { openId: openId }]);
    }

    goShowCard() {
        this.router.navigate(["/members/member-card"]);
    }

    goBindPhone() {
        this.router.navigate(["/personals/bind-member"]);
    }
    goFeedBack() {
        this.router.navigate(['/feedbacks/feedback']);
    }
    goPurchaseRecord(openId: string) {
        this.router.navigate(['/purchaserecords/purchaserecord', { openId: openId }]);
    }
    goShopEvaluation(openId: string) {
        this.router.navigate(['/purchaserecords/purchaserecord', { openId: openId, pageType: 'shopEvaluation' }]);
    }
    goArchivalLevel() {
        //2：内部员工，1：零售用户
        if (this.user.userType == UserType.Staff) {
            this.router.navigate(['/customer-searchs/customer-search']);
        } else if (this.user.userType == UserType.Retailer) {
            this.router.navigate(['/account-levels/account-level']);
        }
        // else{
        //     this.router.navigate(['/customer-searchs/customer-search']);
        // }
    }
    goShopEmployee() {
        //当是店主的时候进入店员管理
        if (this.user.isShopkeeper) {
            this.router.navigate(['/shop-employees/shop-employee', { userId: this.user.userId }]);
        }
    }

    /**
     * 获取未审核店员数
     */
    getNoCheckShopEmployeeCount() {
        this.wechatUserService.getNoCheckShopEmployeeCount({ tenantId: this.settingsService.tenantId, userId: this.user.userId }).subscribe(data => {
            this.noCheckShopEmployeeCount = data;
        });
    }

    goFavorite() {
        this.router.navigate(['/favorites/favorite']);
    }

    goEmpRetailer() {
        this.router.navigate(['/emp-retailers/emp-retailer']);
    }

    //创建新的抽奖
    creatNewLottery() {
        this.router.navigate(['/lotterys/lottery']);
    }
    //查看抽奖活动列表
    showLotteryList() {
        this.router.navigate(['/lotterys/lottery-activities-list']);
    }

    //查看签到部门页 admin
    showSignInPeople() {
        this.router.navigate(['/lotterys/lottery-sign-in-list']);
    }
    //去签到
    gotoSigin() {
        this.router.navigate(['/lotterys/lottery-sign-in']);
    }

    //去抽奖
    goToLuckyDraw() {
        this.router.navigate(['/lotterys/lottery-list']);
    }

    //是否为抽奖活动管理员
    getIsLotteryAdmin() {
        this.wechatUserService.getIsLotteryAdmin(this.user.openId).subscribe(data => {
            this.isLotteryAdmin = data;
        });
    }
} 
