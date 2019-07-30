import { Component, OnInit, Injector, EventEmitter, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Questionnaire } from '@shared/entity/marketting';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { QuestionnaireServiceProxy } from '@shared/service-proxies/marketing-service';
import { AppConsts } from '@shared/AppConsts';
import { ActivatedRoute } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'edit-questionnaire-modal',
    templateUrl: 'edit-questionnaire.component.html',
    styleUrls: ['edit-questionnaire.component.scss']
})
export class EditQuestionnaireComponent extends AppComponentBase implements OnInit{
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    question = new Questionnaire();
    questionsType = [
        { text: '客户服务评价', value: 1 },
        { text: '卷烟供应评价', value: 2 },
        { text: '市场管理评价', value: 3 },
        { text: '综合评价', value: 4 },
    ];
    search: any = { type: null};
    emodalVisible = false;
    isConfirmLoading = false;
    form: FormGroup;
    //用于按钮是否显示
    cardTitle = '';

    constructor(injector: Injector
        , private fb: FormBuilder
        , private questionnaireService:QuestionnaireServiceProxy
        ) {
        super(injector);
    }

    ngOnInit(): void {
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
     * 根据问题id
     * @param id 
     */
    getQuestionById(id: string) {
        this.questionnaireService.get(id).subscribe((result: Questionnaire) => {
            this.question = result;
        });
    }

    /**
     * 显示模态框方法
     * @param id 
     */
    show(id?:string){
        if (!id) {
            this.cardTitle="新增问题";
            this.question=new Questionnaire();
            this.question.isMultiple=false;
        }else{
            this.cardTitle="编辑问题";
            this.getQuestionById(id);
        }
        this.emodalVisible=true;
    }

    /**
     * 取消按钮事件
     */
    ehandleCancel = (e) => {
        this.emodalVisible = false;
        // this.isConfirmLoading = false;
        this.reset(e);
    }
    reset(e?): void {
        if (e) {
            e.preventDefault();
        }
        this.form.reset();
        for (const key in this.form.controls) {
            this.form.controls[key].markAsPristine();
        }

    }

    /**
     * 保存员工信息
     */
    save() {
        this.isConfirmLoading = true;
        //return;
        //检查form验证
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.questionnaireService.create(this.question)
                        .finally(() => {
                            this.isConfirmLoading = false;
                        })
                        .subscribe(data => {
                            if (data.code == 0) {
                                this.notify.success('保存成功！','');
                                this.emodalVisible = false;
                                this.modalSave.emit(null);
                            }else{
                                this.notify.error(data.msg,'');
                            }
                            
                        });
        }
    }
}
