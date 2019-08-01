import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Router } from '@angular/router';
import { QuestionnaireService } from '../../../services';

@Component({
    moduleId: module.id,
    selector: 'questionnaire',
    templateUrl: 'questionnaire.component.html',
    styleUrls: ['questionnaire.component.scss']
})
export class QuestionnaireComponent extends AppComponentBase implements OnInit{

    constructor(injector: Injector
        , private router: Router
        , private questionnaireService: QuestionnaireService) {
        super(injector);
    }

    ngOnInit() {
        
    }

    getQuestionnaireFillRecord(){

    }

    goDetail(quarter: number) {
        this.router.navigate(['/questionnaires/questionnaire-detail', { quarter: quarter }]);
    }
}
