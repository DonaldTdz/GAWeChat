import { Component, ViewEncapsulation, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../../components/app-component-base';
import { QuestionnaireService } from '../../../../services';
import { Router, ActivatedRoute } from '@angular/router';
import { timer } from 'rxjs/observable/timer';
import { Subscription } from 'rxjs';
import { ProgressModule } from 'ngx-weui';
import { Questionnaire, QuestionOptions } from '../../../../services/model';

@Component({
    moduleId: module.id,
    selector: 'questionnaire-detail',
    templateUrl: 'questionnaire-detail.component.html',
    styleUrls: ['questionnaire-detail.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class QuestionnaireDetailComponent extends AppComponentBase implements OnInit {


    progressBar:any = { 
        p1:{ value: 0, doing: false }
     };
    questionnaireList : Questionnaire[] = [];//问题列表
    quarter: string = this.route.snapshot.params['quarter'];//季度

    constructor(injector: Injector
        , private router: Router
        , private route: ActivatedRoute
        , private questionnaireService: QuestionnaireService
    ) {
        super(injector);
    }

    ngOnInit() {
        // this.progressBar = {
        //     p1: { value: 0, doing: true },
        //   };
        //   timer(0, 40).subscribe(() => {
        //     const item = this.progressBar.p1;
        //       ++item.value;
        //       console.log(item.value);
              
        //       if (item.value >= 100) item.doing = false;
        //   });
        this.questionnaireList.push(new Questionnaire({
            id:"1",
            type:1,
            isMultiple:false,
            question:"您是否在xx网站购买过数码家电类产品？",
            no:"01",
            typeName:"11",
            questionOptions:[ QuestionOptions.fromJS({
                value:'1',
                desc:'满意',
                id:'1',
                questionnaireId:'123'
            }),QuestionOptions.fromJS({
                value:'2',
                desc:'不满意',
                id:'2',
                questionnaireId:'123'
            })]
        }));
        this.questionnaireList.push(new Questionnaire({
            id:"2",
            type:1,
            isMultiple:false,
            question:"您在xx网站购买数码家电类产品的次数是？",
            no:"02",
            typeName:"11",
            questionOptions:[]
        }));
        this.questionnaireList.push(new Questionnaire({
            id:"3",
            type:1,
            isMultiple:false,
            question:"到目前为止，您在xx网站购买数码家电类产品有多久了？",
            no:"03",
            typeName:"11",
            questionOptions:[]
        }));
    }

    getQuestionnaireList(){
        
    }

    //提交
    onSave(){

    }
}
