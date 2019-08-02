import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Questionnaire } from '@shared/entity/marketting/questionnaire';
import { EditQuestionnaireComponent } from './edit-questionnaire/edit-questionnaire.component';
import { Router } from '@angular/router';
import { PagedResultDtoOfQuestionnaire, QuestionnaireServiceProxy } from '@shared/service-proxies/marketing-service';
import { NzModalService } from 'ng-zorro-antd';

@Component({
    moduleId: module.id,
    selector: 'questionnaire',
    templateUrl: 'questionnaire.component.html'
})
export class QuestionnaireComponent extends AppComponentBase implements OnInit {
    @ViewChild('editQuestionnaireModal') editQuestionnaireModal: EditQuestionnaireComponent;

    loading = false;
    searchType:number=null;
    questionsType = [
        { text: '客户服务评价', value: 1 },
        { text: '卷烟供应评价', value: 2 },
        { text: '市场管理评价', value: 3 },
        { text: '综合评价', value: 4 },
    ];
    questions : Questionnaire[] = [];
    constructor(injector: Injector
        ,private router:Router
        ,private questionnaireService:QuestionnaireServiceProxy
        , private modal: NzModalService
        ) {
        super(injector);
    }
    ngOnInit(): void {
        this.refreshData(false);
    }

    /**
     * 分页获取问题信息
     * @param reset 是否刷新页面
     */
    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.searchType=null;
            this.query.pageIndex = 1;
        }
        
        this.loading = true;
        this.questionnaireService.getAll(this.query.skipCount(), this.query.pageSize, this.searchType).subscribe((result: PagedResultDtoOfQuestionnaire) => {
            this.loading = false;
            let status = 0;
            this.questions = result.items;
            this.query.total = result.totalCount;
        });
    }

    /**
     * 编辑问题
     */
    editQuestionnaire(id:string) {
        //this.editQuestionnaireModal.show(id);
        this.router.navigate(['admin/marketting/detail-questionnaire',id]);
    }

    /**
     * 新建问题
     */
    create(){
        this.router.navigate(['admin/marketting/create-questionnaire']);
    }

    /**
     * 删除问题
     * @param employee 员工实体
     * @param contentTpl 弹框id
     */
    delete(question: Questionnaire): void {
        this.modal.confirm({
            content: '是否确认删除问题编号'+question.no+'?',
            okText: '是',
            cancelText: '否',
            onOk: () => {
                this.questionnaireService.delete(question.id).subscribe((data) => {
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

}
