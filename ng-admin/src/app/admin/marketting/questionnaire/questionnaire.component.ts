import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Questionnaire } from '@shared/entity/marketting/questionnaire';
import { Router } from '@angular/router';
import { PagedResultDtoOfQuestionnaire, QuestionnaireServiceProxy, PagedResultDtoOfQuestionRecord } from '@shared/service-proxies/marketing-service';
import { NzModalService } from 'ng-zorro-antd';
import { QuestionRecord } from '@shared/entity/marketting';

@Component({
    moduleId: module.id,
    selector: 'questionnaire',
    templateUrl: 'questionnaire.component.html'
})
export class QuestionnaireComponent extends AppComponentBase implements OnInit {

    loading = false;
    searchType:number=null;
    searchQuestionRecordType:number=null;
    queryQuestionRecord: any = {
        pageIndex: 1,
        pageSize: 10,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
        sorter: '',
        status: -1,
        statusList: []
    };
    questionsType = [
        { text: '客户服务评价', value: 1 },
        { text: '卷烟供应评价', value: 2 },
        { text: '市场管理评价', value: 3 },
        { text: '综合评价', value: 4 },
    ];
    quarterType = [
        { text: '第一季度', value: 1 },
        { text: '第二季度', value: 2 },
        { text: '第三季度', value: 3 },
        { text: '第四季度', value: 4 },
    ];
    questionRecords : QuestionRecord[] = [];
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
        this.refreshQuestionRecordData(false);
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

    refreshQuestionRecordData(reset = false, search?: boolean) {
        if (reset) {
            this.searchQuestionRecordType=null;
            this.queryQuestionRecord.pageIndex = 1;
        }
        
        this.loading = true;
        this.questionnaireService.getAllQuestionRecord(this.queryQuestionRecord.skipCount(), this.queryQuestionRecord.pageSize, null, this.searchQuestionRecordType).subscribe((result: PagedResultDtoOfQuestionRecord) => {
            this.loading = false;
            this.questionRecords = result.items;
            //console.log(this.questionRecords);
            
            this.queryQuestionRecord.total = result.totalCount;
        });
    }

    /**
     * 问题详情
     */
    detail(id:string) {
        this.router.navigate(['admin/marketting/detail-questionnaire',id]);
    }

    /**
     * 新建问题
     */
    create(){
        this.router.navigate(['admin/marketting/create-questionnaire']);
    }

    createQuestionRecord(){
        this.router.navigate(['admin/marketting/detail-question-record']);
    }

    /**
     * 问题详情
     */
    detailQuestionRecord(id:string) {
        this.router.navigate(['admin/marketting/detail-question-record',id]);
    }

    /**
     * 删除问题
     * @param question 问题实体
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

    /**
     * 删除问卷记录
     * @param question 问题实体
     */
    deleteQuestionRecord(questionRecord: QuestionRecord): void {
        this.modal.confirm({
            content: '是否确认删除问卷记录'+questionRecord.title+'?',
            okText: '是',
            cancelText: '否',
            onOk: () => {
                this.questionnaireService.deleteQuestionRecord(questionRecord.id).subscribe((data) => {
                    if (data.code == 0) {
                        this.notify.success('删除成功！','');
                        this.refreshQuestionRecordData();
                    }else{
                        this.notify.error(data.msg);
                    }
                })
            }
        })

    }

}
