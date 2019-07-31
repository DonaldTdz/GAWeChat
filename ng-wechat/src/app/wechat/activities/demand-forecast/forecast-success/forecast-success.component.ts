import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'forecast-success',
    templateUrl: 'forecast-success.component.html',
    encapsulation: ViewEncapsulation.None
})
export class ForecastSuccessComponent {
    constructor(private router: Router) {
    }

    goBack() {
        this.router.navigate(['/demand-forecasts/demand-forecast']);
    }
}
