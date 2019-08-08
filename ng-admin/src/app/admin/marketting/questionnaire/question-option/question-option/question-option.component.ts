import { Component, OnInit, Output, EventEmitter, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { QuestionOptions } from '@shared/entity/marketting';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { QuestionnaireServiceProxy } from '@shared/service-proxies/marketing-service';

@Component({
    moduleId: module.id,
    selector: 'question-option-modal',
    templateUrl: 'question-option.component.html',
    styleUrls: ['question-option.component.scss']
})
export class QuestionOptionComponent extends AppComponentBase implements OnInit{
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    questionnaireId:string;
    questionOptions = new QuestionOptions();
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
            value:[null, Validators.compose([Validators.required])],
            desc: [null, Validators.compose([Validators.required])]
        });
    }

    getFormControl(name: string) {
        return this.form.controls[name];
    }

    /**
     * 根据问题id
     * @param id 
     */
    getQuestionOptionById(id: string) {
        this.questionnaireService.getQuestionOptionById(id).subscribe((result: QuestionOptions) => {
            this.questionOptions = result;
            //console.log(this.questionOptions);
            
        });
    }

    /**
     * 显示模态框方法
     * @param id 
     */
    show(questionnaireId:string,id?:string){
        if (!id) {
            this.cardTitle="新增问题配置";
            this.questionOptions=new QuestionOptions();
        }else{
            this.cardTitle="编辑问题配置";
            this.getQuestionOptionById(id);
        }
        this.questionnaireId = questionnaireId;
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
        this.questionOptions.questionnaireId = this.questionnaireId;
        //return;
        //检查form验证
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.questionnaireService.createQuestionOption(this.questionOptions)
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
