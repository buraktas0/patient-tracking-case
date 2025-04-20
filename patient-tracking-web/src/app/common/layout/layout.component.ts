import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from '../header/header.component';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    imports: [RouterModule, HeaderComponent]
})
export class LayoutComponent { }