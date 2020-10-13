import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CommonModule} from '@angular/common';
import {SubmitSolutionComponent} from './submit-solution/submit-solution.component';
import {AddNewProblemComponent} from './add-new-problem/add-new-problem.component';
import {DeleteProblemComponent} from './delete-problem/delete-problem.component';

const routes: Routes = [
  {path: '', component: SubmitSolutionComponent},
  {path: 'Problems', component: SubmitSolutionComponent},
  {path: '+', component: AddNewProblemComponent},
  {path: '-', component: DeleteProblemComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
