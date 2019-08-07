import { Component, ViewEncapsulation, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { QuestionnaireService } from '../../../../services';
import { Router, ActivatedRoute } from '@angular/router';
import { ToptipsService } from 'ngx-weui';
import { Questionnaire, QuestionAnswer } from '../../../../services/model';

@Component({
    moduleId: module.id,
    selector: 'questionnaire-detail',
    templateUrl: 'questionnaire-detail.component.html',
    styleUrls: ['questionnaire-detail.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class QuestionnaireDetailComponent extends AppComponentBase implements OnInit {
    progressBar: number = 0;//进度条
    checkedNum: number = 0;//已选择个数
    status: boolean = false;//是否可填写
    questionnaireList: Questionnaire[] = [];//问题列表
    questionRecordId: string = this.route.snapshot.params['id'];
    statusParams: string = this.route.snapshot.params['status'];
    constructor(injector: Injector
        , private router: Router
        , private route: ActivatedRoute
        , private questionnaireService: QuestionnaireService
        , private srv: ToptipsService
    ) {
        super(injector);
    }

    ngOnInit() {
        if (this.statusParams == 'true') {
            this.status = true;
            this.getQuestionnaireList();
        } else {
            this.status = false;
            this.progressBar=100;
            this.getQuestionRecordById();
        }
    }

    //获取填写记录
    getQuestionRecordById() {
        let params: any = {};
        params.openId = this.settingsService.openId;
        params.questionRecordId = this.questionRecordId;
        this.questionnaireService.getQuestionRecordById(params).subscribe(data => {
            this.questionnaireList = data;
            //多选项的答案填充到数组下
            let multipleList = this.questionnaireList.filter(i => i.isMultiple == true);
            multipleList.forEach(m => {
                m.values = m.value.split(',');
            })
            let remarkQues = this.questionnaireList.filter(i=>i.remark !== null);
            remarkQues.forEach(r=>{
                r.desc = r.remark;
            })
        });
    }

    //获取问题列表
    getQuestionnaireList() {
        this.questionnaireService.getQuestionnaireList().subscribe(data => {
            this.questionnaireList = data;
            //过滤掉跳题问题
            let disabledQues = this.questionnaireList.filter(i => i.no.split('.').length > 1);
            disabledQues.forEach(d => d.enabled = false);
            
        });
    }

    //单选复选框改变事件
    onRadioChange(index, desc?: string, value?:string, values?:string[]) {
        if(!this.status){
            return;
        }
        if (desc && desc == '其它（请注明）') {
            this.questionnaireList[index].desc = desc;
        } else {
            this.questionnaireList[index].desc = null;
        }
        if (value){
            this.JumpTopic(index,value);
        }
        
        this.progress(index,values);
        this.progressShow();
    }

    ///进度条逻辑
    progress(index,values) {
        // if(this.questionnaireList[index].isChecked){
        //     return;
        // }
        
        // this.questionnaireList[index].isChecked = true;
        // this.checkedNum++;
        // return;
        if(this.questionnaireList[index].isMultiple){
            if(this.questionnaireList[index].isChecked){
                if(values.length < 1){
                    this.questionnaireList[index].isChecked = false;
                    this.checkedNum--;
                }
            }else{
                this.questionnaireList[index].isChecked = true;
                this.checkedNum++;
            }
        }else{
            if(this.questionnaireList[index].isChecked){
                return;
            }
            
            this.questionnaireList[index].isChecked = true;
            this.checkedNum++;
        }
        
    }

    //进度条进度展示
    progressShow(){
        this.checkedNum = this.questionnaireList.filter(v=>v.enabled==true&&v.isChecked==true).length;
        let enabledCount: number = this.questionnaireList.filter(v => v.enabled == true).length;
        this.progressBar = (this.checkedNum / enabledCount) * 100;
        //console.log(this.checkedNum+"-----"+ enabledCount);
    }

    //跳转题目处理逻辑
    JumpTopic(index,value:string) {
        if (this.questionnaireList[index].no == "Q8") {
            if(value == "1"){
                this.questionnaireList[index + 1].enabled = true;
            }else{
                this.questionnaireList[index + 1].enabled = false;
            }
        }
        if (this.questionnaireList[index].no == "Q11") {
            if(value == "3" || value == "4" || value == "5")
            {
                this.questionnaireList[index + 1].enabled = true;
                this.questionnaireList[index + 2].enabled = true;
            }else if(this.questionnaireList[index].isChecked && (value == "1" || value == "2")){
                this.questionnaireList[index + 1].enabled = false;
                this.questionnaireList[index + 2].enabled = false;
            }
        }
        if (this.questionnaireList[index].no == "Q13") {
            this.questionnaireList[index + 1].enabled = true;
            this.questionnaireList[index + 2].enabled = true;
        }
        if (this.questionnaireList[index].no == "Q20") {
             if(value =="4"||value == "5"){
                this.questionnaireList[index + 1].enabled = true;
             }else{
                this.questionnaireList[index + 1].enabled = false;
             }
        }
    }

    save() {
        if (this.progressBar < 100) {
            this.srv['warn']('还有未选择的项');
            return;
        }
        let checkBoxList: Questionnaire[] = this.questionnaireList.filter(v => v.isMultiple == true);
        checkBoxList.forEach(element => {
            if (element.isMultiple) {
                element.value = element.values.join(',');
            }
        });
        let input: any = {};
        input.openId = this.settingsService.openId;
        input.questionRecordId = this.questionRecordId;
        //仅提交填写启用的问题
        let answerRecords:Questionnaire[] = this.questionnaireList.filter(i=>i.enabled==true);
        input.list = QuestionAnswer.fromJSArray(answerRecords);
        //console.log(input)
        //return;
        this.questionnaireService.createAnswerRecord(input).subscribe(data => {
            if (data && data.code == 0) {
                this.router.navigate(['/questionnaires/submit-success']);
            } else {
                this.srv['warn']('提交失败，请重试');
            }
        });
    }
}
