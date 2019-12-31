import { HomeComponent } from './home/home.component';

export const routes = [
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: 'home', component: HomeComponent },

    { path: 'activities', loadChildren: './activities/activities.module#ActivitiesModule' },
    { path: 'shares', loadChildren: './activities/share/share.module#ShareModule' },
    { path: 'members', loadChildren: './personal-center/member-card/member-card.module#MemberCardModule' },
    { path: 'personals', loadChildren: './personal-center/personal/personal.module#PersonalModule' },
    { path: 'shops', loadChildren: './personal-center/shop/shop.module#ShopModule' },
    { path: 'shopadds', loadChildren: './personal-center/shop/shop-add/shop-add.module#ShopAddModule' },
    { path: 'scans', loadChildren: './personal-center/scan/scan.module#ScanModule' },
    { path: 'nearbies', loadChildren: './buy/nearby-shop/nearby-shop.module#NearbyShopModule' },
    { path: 'feedbacks', loadChildren: './personal-center/feedback/feedback.module#FeedbackModule' },
    { path: 'goodses', loadChildren: './buy/goods/goods.module#GoodsModule' },
    { path: 'integrals', loadChildren: './personal-center/integral/integral.module#IntegralModule' },
    { path: 'purchaserecords', loadChildren: './personal-center/purchaserecord/purchaserecord.module#PurchaserecordModule' },
    { path: 'customer-searchs', loadChildren: './personal-center/customer-search/customer-search.module#CustomerSearchModule' },
    { path: 'account-levels', loadChildren: './personal-center/account-level/account-level.module#AccountLevelModule' },
    { path: 'shopevaluations', loadChildren: './personal-center/shopevaluation/shopevaluation.module#ShopEvaluationModule' },
    { path: 'good-sources', loadChildren: './personal-center/good-source/good-source.module#GoodSourceModule' },
    { path: 'shop-employees', loadChildren: './personal-center/shop-employee/shop-employee.module#ShopEmployeeModule' },
    //{ path: 'imgs', loadChildren: './personal-center/img-cropper/img-cropper.module#ImgCropperModule' },
    // Not found
    // { path: 'lotteries', loadChildren: './activities/lottery/lottery.module#LotteryModule' },
    { path: 'exhibitions', loadChildren: './activities/exhibition/exhibition.module#ExhibitionModule' },
    { path: 'favorites', loadChildren: './personal-center/favorite/favorite.module#FavoriteModule' },
    { path: 'questionnaires', loadChildren: './activities/questionnaire/questionnaire.module#QuestionnaireModule' },
    { path: 'demand-forecasts', loadChildren: './activities/demand-forecast/demand-forecast.module#DemandForecastModule' },
    { path: 'emp-retailers', loadChildren: './personal-center/retailer-manage/retailer-manage.module#RetailerManageModule' },
    { path: 'lotterys', loadChildren: './activities/lucky-draw/lottery.module#LotteryModule' },
    { path: '**', redirectTo: '' }
];

