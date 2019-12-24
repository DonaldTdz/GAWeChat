import { Component, ViewEncapsulation, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '../../components/app-component-base';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'lucky-draw',
    templateUrl: 'lucky-draw.component.html',
    styleUrls: ['lucky-draw.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class LuckyDrawComponent extends AppComponentBase implements OnInit {

    constructor(injector: Injector
        , private router: Router) {
        super(injector);
    }
    ngOnInit() {
    }
}