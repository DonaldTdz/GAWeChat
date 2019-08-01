import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Questionnaire, QuestionOptions } from '@shared/entity/marketting';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { QuestionnaireServiceProxy } from '@shared/service-proxies/marketing-service';
import { Router, ActivatedRoute } from '@angular/router';
import { QuestionOptionComponent } from '../../question-option/question-option/question-option.component';
import { NzModalService } from 'ng-zorro-antd';

@Component({
    moduleId: module.id,
    selector: 'detail-questionnaire',
    templateUrl: 'detail-questionnaire.component.html',
    styleUrls: ['detail-questionnaire.component.scss']
})
export class DetailQuestionnaireComponent extends AppComponentBase implements OnInit{
    @ViewChild('questionOptionModal') questionOptionModal: QuestionOptionComponent;
    questionId:string;
    question = new Questionnaire();
    questionOptions:QuestionOptions[];
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
        , private actRouter: ActivatedRoute
        , private router: Router
        , private questionnaireService:QuestionnaireServiceProxy
        , private modal: NzModalService
        ) {
        super(injector);
        this.questionId = this.actRouter.snapshot.params['id'];
    }

    ngOnInit(): void {
        this.form = this.fb.group({
            no:[null, Validators.compose([Validators.required])],
            question: [null, Validators.compose([Validators.required])],
            isMultiple:[null, Validators.required],
            type:[null, Validators.required]
        });
        this.getQuestionById(this.questionId);
    }

    getQuestionById(id: string) {
        this.questionnaireService.get(id).subscribe((result: Questionnaire) => {
            this.question = result;
        });
        this.getQuestionOptionsListById(id);
    }

    //答案配置列表
    getQuestionOptionsListById(id: string){
        this.questionnaireService.getQuestionOptionsListById(id).subscribe((result: QuestionOptions[]) => {
            this.questionOptions = result;
        });
        console.log(this.questionOptions);
        
    }

    getFormControl(name: string) {
        return this.form.controls[name];
    }

    /**
     * 保存员工信息
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

    /**
     * 删除问题
     * @param employee 员工实体
     * @param contentTpl 弹框id
     */
    delete(option: QuestionOptions): void {
        this.modal.confirm({
            content: '是否确认删除选项'+option.value+'?',
            okText: '是',
            cancelText: '否',
            onOk: () => {
                this.questionnaireService.deleteQuestionOptionsById(option.id).subscribe((data) => {
                    if (data.code == 0) {
                        this.notify.success('删除成功！','');
                        this.refreshData();
                    }else{
                        this.notify.error(data.msg);
                    }
                })
            }
        })

    }

    editQuestionOption(id:string){
        this.questionOptionModal.show(this.questionId,id);
    }

    createQuestionOption(){
        this.questionOptionModal.show(this.questionId);
    }

    refreshData(){
        this.getQuestionOptionsListById(this.questionId);
    }

    return() {
        this.router.navigate(['admin/marketting/questionnaire']);
    }
}
