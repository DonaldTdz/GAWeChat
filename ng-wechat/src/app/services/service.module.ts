import { NgModule } from '@angular/core';
import { HttpClient } from './httpclient';
import { WechatUserService, SettingsService, ShopService, FeedBackService, ArticleService, IntegralDetailService, CommonService, CustomerService, LevelAccountAccpintService, FavoriteService, DemandForecastService } from './index';
import { GoodSourceService } from './personal-center/good-source.service';

@NgModule({
        providers: [
                HttpClient,
                WechatUserService,
                SettingsService,
                ShopService,
                FeedBackService,
                ArticleService,
                IntegralDetailService,
                CustomerService,
                CommonService,
                LevelAccountAccpintService,
                GoodSourceService,
                FavoriteService,
                DemandForecastService,
        ]
})
export class ServiceModule { }