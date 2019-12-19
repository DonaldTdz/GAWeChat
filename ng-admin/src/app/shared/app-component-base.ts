import { Injector, ElementRef } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { LocalizationService } from '@abp/localization/localization.service';
import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import { FeatureCheckerService } from '@abp/features/feature-checker.service';
import { NotifyService } from '@abp/notify/notify.service';
import { SettingService } from '@abp/settings/setting.service';
import { MessageService } from '@abp/message/message.service';
import { AbpMultiTenancyService } from '@abp/multi-tenancy/abp-multi-tenancy.service';
import { AppSessionService } from '@shared/session/app-session.service';
//import { DatePipe } from '@angular/common';

export abstract class AppComponentBase {

    localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;

    query: any = {
        pageIndex: 1,
        pageSize: 10,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
        sorter: '',
        status: -1,
        statusList: []
    };

    localization: LocalizationService;
    permission: PermissionCheckerService;
    feature: FeatureCheckerService;
    notify: NotifyService;
    setting: SettingService;
    message: MessageService;
    multiTenancy: AbpMultiTenancyService;
    appSession: AppSessionService;
    elementRef: ElementRef;

    constructor(injector: Injector) {
        this.localization = injector.get(LocalizationService);
        this.permission = injector.get(PermissionCheckerService);
        this.feature = injector.get(FeatureCheckerService);
        this.notify = injector.get(NotifyService);
        this.setting = injector.get(SettingService);
        this.message = injector.get(MessageService);
        this.multiTenancy = injector.get(AbpMultiTenancyService);
        this.appSession = injector.get(AppSessionService);
        this.elementRef = injector.get(ElementRef);
    }

    l(key: string, ...args: any[]): string {
        let localizedText = this.localization.localize(key, this.localizationSourceName);

        if (!localizedText) {
            localizedText = key;
        }

        if (!args || !args.length) {
            return localizedText;
        }

        args.unshift(localizedText);
        return abp.utils.formatString.apply(this, args);
    }

    isGranted(permissionName: string): boolean {
        return this.permission.isGranted(permissionName);
    }

    dateFormat(date: any): string {
        if (date === null) {
            return null;
        }
        let d = new Date(date);
        let y = d.getFullYear().toString();
        let m = (d.getMonth() + 1).toString();
        let day = d.getDate().toString();
        return y + '-' + m + '-' + day;
        //let dateStr:string = this.datePipe.transform(d,'yyyy-MM-dd');
        //return dateStr;
    }
    dateFormatHH(date: any): string {
        if (date === null) {
            return null;
        }
        let d = new Date(date);
        let y = d.getFullYear().toString();
        let m = (d.getMonth() + 1).toString();
        let day = d.getDate().toString();
        let h = d.getHours();
        let ms = d.getMinutes();
        let hh = h > 10 ? h.toString() : '0' + h.toString();
        let mm = ms > 10 ? ms.toString() : '0' + ms.toString();
        return y + '-' + m + '-' + day + ' ' + hh + ':' + mm;
        // let dateStr:string = this.datePipe.transform(d,'yyyy-MM-dd');
        //return dateStr;
    }

    dateFormatMM(date: any): string {
        if (date === null) {
            return null;
        }
        let d = new Date(date);
        let y = d.getFullYear().toString();
        let m = (d.getMonth() + 1).toString();
        return y + '-' + m;
    }

}
