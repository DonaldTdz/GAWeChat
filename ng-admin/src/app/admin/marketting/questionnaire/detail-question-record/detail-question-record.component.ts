import { Component, Injector, OnInit } from '@angular/core';
import { QuestionRecord } from '@shared/entity/marketting';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionnaireServiceProxy } from '@shared/service-proxies/marketing-service';
import { AppComponentBase } from '@shared/app-component-base';
import { Router, ActivatedRoute } from '@angular/router';
import { NzModalService } from 'ng-zorro-antd';

@Component({
    moduleId: module.id,
    selector: 'detail-question-record',
    templateUrl: 'detail-question-record.component.html',
    styleUrls: ['detail-question-record.component.scss']
})
export class DetailQuestionRecordComponent extends AppComponentBase implements OnInit {
    questionRecord = new QuestionRecord();
    id: string;
    form: FormGroup;
    quarterType = [
        { text: '第一季度', value: 1 },
        { text: '第二季度', value: 2 },
        { text: '第三季度', value: 3 },
        { text: '第四季度', value: 4 },
    ];
    //用于按钮是否显示
    cardTitle = '';

    constructor(injector: Injector
        , private fb: FormBuilder
        , private router: Router
        , private questionnaireService: QuestionnaireServiceProxy
        , private actRouter: ActivatedRoute
        , private modal: NzModalService
    ) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }

    ngOnInit(): void {

        this.form = this.fb.group({
            title: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            quarter:[null,Validators.required]
        });
        if (!this.id) {
            this.cardTitle = "新增问卷规则";

            let date = new Date();
            let year = date.getFullYear();
            this.questionRecord.year = year.toString();
            this.questionRecord.isPublish=false;
        } else {
            this.cardTitle = "问卷规则详情";
            this.getQuestionRecordById();
        }
    }

    getFormControl(name: string) {
        return this.form.controls[name];
    }

    getQuestionRecordById() {
        this.questionnaireService.getQuestionRecordById(this.id).subscribe(res=>{
            this.questionRecord = res;
        });
    }

    /**
     * 保存问卷信息
     */
    save() {
        //return;
        //检查form验证
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.questionnaireService.createQuestionRecord(this.questionRecord)
                .finally(() => {

                })
                .subscribe(data => {
                    if (data.code == 0) {
                        this.questionRecord = data.data;
                        let msg = this.questionRecord.isPublish == false ? '保存成功！' : '发布成功！';
                        this.notify.success(msg, '');
                    } else {
                        this.notify.error(data.msg, '');
                    }
                });
        }
    }

    push() {
        this.modal.confirm({
            content: '发布后不可修改，是否确认发布?',
            cancelText: '否',
            okText: '是',
            onOk: () => {
                this.questionRecord.isPublish = true;
                this.save();
            }
        });
    }

    return() {
        this.router.navigate(['admin/marketting/questionnaire']);
    }
}
