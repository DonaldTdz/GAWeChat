import { Component, Injector, OnInit } from '@angular/core';
import { Questionnaire } from '@shared/entity/marketting';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuestionnaireServiceProxy } from '@shared/service-proxies/marketing-service';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'create-questionnaire',
    templateUrl: 'create-questionnaire.component.html',
    styleUrls: ['create-questionnaire.component.scss']
})
export class CreateQuestionnaireComponent extends AppComponentBase implements OnInit{
    question = new Questionnaire();
    questionsType = [
        { text: '客户服务评价', value: 1 },
        { text: '卷烟供应评价', value: 2 },
        { text: '市场管理评价', value: 3 },
        { text: '综合评价', value: 4 },
    ];
    search: any = { type: null};
    form: FormGroup;
    //用于按钮是否显示
    cardTitle = '';

    constructor(injector: Injector
        , private fb: FormBuilder
        , private router: Router
        , private questionnaireService:QuestionnaireServiceProxy
        ) {
        super(injector);
    }

    ngOnInit(): void {
        this.question.isMultiple=false;
        this.cardTitle="新增问题";
        this.form = this.fb.group({
            no:[null, Validators.compose([Validators.required])],
            question: [null, Validators.compose([Validators.required])],
            isMultiple:[null, Validators.required],
            type:[null, Validators.required]
        });
    }

    getFormControl(name: string) {
        return this.form.controls[name];
    }

    /**
     * 保存问题信息
     */
    save() {
        //return;
        //检查form验证
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.questionnaireService.create(this.question)
                        .finally(() => {

                        })
                        .subscribe(data => {
                            if (data.code == 0) {
                                this.notify.success('保存成功！','');
                                this.return();
                            }else{
                                this.notify.error(data.msg,'');
                            }
                        });
        }
    }

    return() {
        this.router.navigate(['admin/marketting/questionnaire']);
    }
}
