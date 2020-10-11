import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import { ScrollingModule } from '@angular/cdk/scrolling';

import {AppComponent} from './app.component';
import {SubmitFormComponent} from './submit-form/submit-form.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { NgxTextDiffModule } from 'ngx-text-diff';
import { SolutionResultsComponent } from './solution-results/solution-results.component';

@NgModule({
  declarations: [
    AppComponent,
    SubmitFormComponent,
    SolutionResultsComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ScrollingModule,
    NgxTextDiffModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
}
