import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { MessageEmployeeModalComponent } from '../member-setting/message-employee-modal/message-employee-modal.component';
import { MemberConfigs } from '@shared/entity/member/memberconfig';
import { ConfigCode } from '@shared/entity/member/configcode';
import { WechatUser, WechatUserDto } from '@shared/entity/wechat';
import { NzModalService } from 'ng-zorro-antd';
import { MemberConfigsServiceProxy } from '@shared/service-proxies/member/memberconfigs-service';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
    moduleId: module.id,
    selector: 'lottery-setting',
    templateUrl: 'lottery-setting.component.html',
})
export class LotterySettingComponent extends AppComponentBase implements OnInit {
    @ViewChild('selectsEmployeeModal') selectsEmployeeModal: MessageEmployeeModalComponent;
    employeeOpenId: string[] = [];
    stringOpenId: string = '';
    stringName: string = '';
    employeeIds: string[] = [];
    form: FormGroup;
    config: MemberConfigs = new MemberConfigs();
    users: WechatUser[] = [];
    usersDto: WechatUserDto[] = [];
    wxloading = false;
    constructor(injector: Injector
        , private fb: FormBuilder
        , private memberconfigsService: MemberConfigsServiceProxy
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.form = this.fb.group({});
        this.getLotteryConfigs();
    }

    getLotteryConfigs() {
        this.memberconfigsService.getLotteryConfigs().subscribe((result: MemberConfigs) => {
            this.config = result;
            this.usersDto = [];
            let splOpenId = [];
            let splName = [];
            if (this.config.desc != null && this.config.value.length != 0) {
                splOpenId = this.config.value.split(',');
                splName = this.config.desc.split(',');
            }
            splOpenId.forEach((v, index) => {
                this.usersDto.push({
                    openId: splOpenId[index],
                    userName: splName[index]
                })
            })
        });
    }

    /**
     * 保存微信用户
     */
    saveConfig() {
        this.stringName = '';
        this.stringOpenId = '';

        this.usersDto.forEach(v => {
            this.stringOpenId += v.openId + ',';
        });
        this.usersDto.forEach(v => {
            this.stringName += v.userName + ',';
        });
        if (this.stringOpenId != null || this.stringOpenId.length >= 0) {
            this.config.value = this.stringOpenId.substring(0, this.stringOpenId.length - 1);
        }
        if (this.stringName != null || this.stringName.length != 0) {
            this.config.desc = this.stringName.substring(0, this.stringName.length - 1);
        }
        this.config.type = 6;
        this.config.code = 11;
        this.wxloading = true;
        this.memberconfigsService.updateLotteryConfig(this.config).subscribe(() => {
            this.notify.success('保存成功！');
            this.getLotteryConfigs();
            this.wxloading = false;
        });
    }

    /**
 * 显示员工列表模态框
 */
    employee(): void {
        this.selectsEmployeeModal.show();
    }

    cancel() {
        this.usersDto = [];
        this.users = [];
        this.stringName = '';
        this.stringOpenId = '';
    }

    existsEmployee(openId: string): boolean {
        let bo = false;
        this.usersDto.forEach(element => {
            if (element.openId == openId) {
                bo = true;
                return;
            }
        });
        return bo;
    }

    getSelectData = (employees?: WechatUser[]) => {
        employees.forEach(element => {
            if (!this.existsEmployee(element.openId)) {
                this.usersDto.push({ openId: element.openId, userName: element.userName });
            }
        });
    }

    onClose(event: Event, openId: string): void {
        let i = 0;
        this.usersDto.forEach(element => {
            if (element.openId == openId) {
                this.usersDto.splice(i, 1);
                return;
            }
            i++;
        });
    }
}
