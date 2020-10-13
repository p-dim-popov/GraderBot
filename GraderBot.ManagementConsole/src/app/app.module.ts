import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { ScrollingModule } from '@angular/cdk/scrolling';

import {AppComponent} from './app.component';
import {SubmitSolutionComponent} from './submit-solution/submit-solution.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { NgxTextDiffModule } from 'ngx-text-diff';
import { SolutionResultsComponent } from './solution-results/solution-results.component';
import { AppRoutingModule } from './app-routing.module';
import { AddNewProblemComponent } from './add-new-problem/add-new-problem.component';
import { DeleteProblemComponent } from './delete-problem/delete-problem.component';

@NgModule({
  declarations: [
    AppComponent,
    SubmitSolutionComponent,
    SolutionResultsComponent,
    AddNewProblemComponent,
    DeleteProblemComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ScrollingModule,
    NgxTextDiffModule,
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
