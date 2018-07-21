import { NgModule } from '@angular/core';

import * as ApiServiceProxies from './service-proxies';
import * as MarketingServiceProxies from './marketing-service/employee-service';
import { ActivityFormServiceProxy } from './marketing-service/activity-form-service';
import { ActivityServiceProxy } from '@shared/service-proxies/marketing-service/acticity-service';
import { ActivityGoodsServiceProxy } from '@shared/service-proxies/marketing-service/activity-goods-service';
import { ActivityBanquetServiceProxy, ActivityDeliveryInfoServiceProxy, UserAnswerService, UserQuestionService, ArticleServiceProxy, ExhibitionShopServiceProxy } from '@shared/service-proxies/marketing-service';
import { RetailCustomerServiceProxy } from '@shared/service-proxies/customer-service/retail-cunstomer-service';
import { AdviseService } from '@shared/service-proxies/consumer-service';
import { WechatUserServiceProxy, WeChatGroupServiceProxy, PurchaseRecordServiceProxy } from '@shared/service-proxies/wechat-service';
import { ManuscriptServiceProxy } from '@shared/service-proxies/marketing-service/manuscript-service';
import { ShopServiceProxy } from '@shared/service-proxies/customer-service/shop-service';
import { ShopEvaluationServiceProxy } from '@shared/service-proxies/customer-service/shop-evaluation-service';
import { ShopProductsServiceProxy, ProductsServiceProxy, GoodSourceServiceProxy } from '@shared/service-proxies/customer-service';
import { IntegralServiceProxy, LotterySettingServiceProxy, WinningRecordServiceProxy } from '@shared/service-proxies/member';
import { MemberConfigsServiceProxy } from '@shared/service-proxies/member/memberconfigs-service';

@NgModule({
    providers: [
        ApiServiceProxies.RoleServiceProxy,
        ApiServiceProxies.SessionServiceProxy,
        ApiServiceProxies.TenantServiceProxy,
        ApiServiceProxies.UserServiceProxy,
        ApiServiceProxies.TokenAuthServiceProxy,
        ApiServiceProxies.AccountServiceProxy,
        ApiServiceProxies.ConfigurationServiceProxy,
        ApiServiceProxies.DriverServiceProxy,
        ApiServiceProxies.AuthSettingServiceProxy,
        ApiServiceProxies.MessageServiceProxy,
        ApiServiceProxies.SubscribeServiceProxy,
        MarketingServiceProxies.EmployeeServiceProxy,
        ApiServiceProxies.EmployeesServiceProxy,
        ActivityServiceProxy,
        ApiServiceProxies.ActivityServiceProxy,
        ActivityFormServiceProxy,
        ActivityGoodsServiceProxy,
        ActivityBanquetServiceProxy,
        ActivityDeliveryInfoServiceProxy,
        RetailCustomerServiceProxy,
        WechatUserServiceProxy,
        UserQuestionService,
        UserAnswerService,
        AdviseService,
        ArticleServiceProxy,
        ManuscriptServiceProxy,
        ShopServiceProxy,
        ShopEvaluationServiceProxy,
        ShopProductsServiceProxy,
        ProductsServiceProxy,
        WeChatGroupServiceProxy,
        IntegralServiceProxy,
        MemberConfigsServiceProxy,
        GoodSourceServiceProxy,
        PurchaseRecordServiceProxy,
        LotterySettingServiceProxy,
        WinningRecordServiceProxy,
        ExhibitionShopServiceProxy,
    ]
})
export class ServiceProxyModule { }