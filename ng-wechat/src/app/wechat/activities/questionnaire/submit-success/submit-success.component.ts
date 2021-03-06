import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'submit-success',
    templateUrl: 'submit-success.component.html'
})
export class SubmitSuccessComponent {
    constructor(private router: Router) {
    }

    goBack() {
        this.router.navigate(['/questionnaires/questionnaire']);
    }
}
