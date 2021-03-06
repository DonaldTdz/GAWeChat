import { Component, ViewEncapsulation, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Observable } from 'rxjs';
// import { ExhibitionShop } from '../../../services/model';
import { Router } from '@angular/router';
import { ArticleService, AppConsts } from '../../../services';
import { ExhibitionShop, Exhibition, VoteLog } from '../../../services/model';
import { ToptipsService, ToastComponent, ToastService, DialogComponent, DialogConfig, SkinType, DialogService } from 'ngx-weui';

@Component({
    moduleId: module.id,
    selector: 'exhibition',
    templateUrl: 'exhibition.component.html',
    styleUrls: ['exhibition.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class ExhibitionComponent extends AppComponentBase implements OnInit {
    shops: Observable<ExhibitionShop[]>;
    value: string;
    exhibitionShopList = [];
    exhibition: Exhibition = new Exhibition();
    picIds: string[] = [];
    voteDesc: string[] = [];
    voteLog: VoteLog = new VoteLog();
    shopTotal: number = 0; // 参赛数
    voteTotal: number = 0; // 投票数
    currentDayVote: number = 0; // 当前用户投票数
    hostUrl: string = AppConsts.remoteServiceBaseUrl;
    qjyyPic: string = null;
    DEFCONFIG: DialogConfig = <DialogConfig>{};
    content = '';
    shareConfig: DialogConfig = {};
    config: DialogConfig = {};
    isAttention: boolean = false; // 用户是否关注
    @ViewChild('ios') iosAS: DialogComponent;
    @ViewChild('success') successToast: ToastComponent;
    selectedType: string = 'vote';
    constructor(injector: Injector, private router: Router, private srvt: ToastService, private srv: ToptipsService, private articleService: ArticleService
        , private dia: DialogService) {
        super(injector);
    }
    ngOnInit() {
        this.qjyyPic = this.hostUrl + '/assets/img/weixin.jpg';
        this.getShopTotal();
        this.getVoteTotal();
        this.getCurrentDayVoteByIdAsync();
        this.getExhibitionConfig();
        this.getExhibitionShop('NoId');
    }

    onShowBySrv(type: SkinType, backdrop: boolean = true) {
        this.DEFCONFIG = <DialogConfig>{
            confirm: '注册会员',
        };
        this.config = Object.assign({}, this.DEFCONFIG, <DialogConfig>{
            skin: type,
            backdrop: backdrop,
            content: '已超过每日投票限制，请明天再来吧！温馨提示：注册会员，买烟积分，享更多活动机会，没注册的小伙伴赶快去注册吧！'
        });
        this.dia.show(this.config).subscribe((res: any) => {
            if (res.value == true) {
                this.router.navigate(["/members/member-card"]);
            }
        });
        return false;
    }
    getExhibitionConfig() {
        let params: any = {};
        this.articleService.GetExhibitionConfigAsync(params).subscribe(result => {
            this.exhibition = result;
            if (this.exhibition.desc != '') {
                this.voteDesc = this.exhibition.desc.split('#');
            }
        });
    }

    getIsAttentionByOpenIdAsync() {
        let params: any = {};
        params.openId = this.settingsService.openId;
        this.articleService.GetIsAttentionByOpenIdAsync(params).subscribe(result => {
            this.isAttention = result;
        });
    }

    getVoteTotal() {
        let params: any = {};
        this.articleService.GetWXVotesCountAsync(params).subscribe(result => {
            this.voteTotal = result;
        });
    }

    getCurrentDayVoteByIdAsync() {
        let params: any = {};
        params.openId = this.settingsService.openId;
        this.articleService.GetCurrentDayVoteByIdAsync(params).subscribe(result => {
            this.currentDayVote = result;
        });
    }

    getShopTotal() {
        let params: any = {};
        this.articleService.GetWXExhibitionShopsCountAsync(params).subscribe(result => {
            this.shopTotal = result;
        });
    }

    getExhibitionShop(type: string) {
        this.exhibitionShopList = []
        this.selectedType = type;
        let params: any = { type: type };
        this.articleService.GetWXPagedExhibitionShopsAsync(params).subscribe(result => {
            this.exhibitionShopList = result;
            result.filter(v => {
                v.items.map(i => {
                    if (i.picPath != '') {
                        let picIds = i.picPath.split(',');
                        i.picPath = picIds[0];
                    }
                });
            })
        });
    }

    onSearch(term: string) {
        this.value = term;
        if (term) {
            this.shops = this.articleService.GetExhibitionShopByKeyAsync({ key: term });
        }
    }

    onCancel() {
    }

    onClear() {
    }

    onSubmit(value: string) {
        console.log('onSubmit', value);
    }

    voteAdd(id: string, type: 'success' | 'loading', forceHide: boolean = false) {
        var currentTime = new Date();
        var bdate = new Date(this.exhibition.beginTime);
        var benginDate = new Date(bdate.getFullYear(), bdate.getMonth(), bdate.getDate(), 0, 0, 0);//开始时间戳
        //alert(benginDate);
        //alert(currentTime);
        //alert(benginDate.valueOf());
        //alert(benginDate.getTime());
        //alert(currentTime.getTime());
        if (benginDate < currentTime) {
            this.articleService.GetIsAttentionByOpenIdAsync(this.settingsService.openId).subscribe(result => {
                this.isAttention = result;
                if (this.isAttention == true) {
                    if (this.currentDayVote < this.exhibition.frequency) {
                        this.voteBLL(id);
                    } else {
                        this.onShowBySrv('ios', false);
                    }
                } else {
                    location.href = this.hostUrl + '/GAWX/QrCode?url=' + encodeURIComponent(this.qjyyPic);
                }
            });
        } else {
            this.srvt['loading']('活动尚未开始哦', 0);
        }
    }

    voteBLL(item: any) {
        item.votes++;
        this.voteTotal++;
        this.currentDayVote++;
        this.voteLog.openId = this.settingsService.openId;
        this.voteLog.exhibitionId = item.id;
        this.articleService.AddVoteLogAsync(this.voteLog).subscribe(data => {
            if (data && data.code === 0) {
                this.srvt['success']('投票成功', 0);
            } else if (data && data.code === 999) {
                this.voteTotal--;
                item.votes--;
                this.currentDayVote--;
                this.srvt['loading']('活动已过期', 0);
            } else if (data && data.code === 888) {
                this.voteTotal--;
                item.votes--;
                this.currentDayVote--;
                this.srvt['loading']('活动尚未开始哦', 0);
            }
            else {
                this.voteTotal--;
                item.votes--;
                this.currentDayVote--;
                this.srvt['loading']('请重试');
            }
        });
    }

    goDetail(shopId: string) {
        this.router.navigate(['/exhibitions/exhibition-detail', { shopId: shopId }]);
    }
}
