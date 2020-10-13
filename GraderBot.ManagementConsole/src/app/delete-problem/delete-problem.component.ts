import { Component, OnInit } from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {SolutionService} from '../solution.service';
import {debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
import {ProblemService} from '../problem.service';

@Component({
  selector: 'app-delete-problem',
  templateUrl: './delete-problem.component.html',
  styleUrls: ['./delete-problem.component.css']
})
export class DeleteProblemComponent implements OnInit {

  apps = {
    'Java Console Application': 'JavaConsoleApp',
    'Java Unit Tested Application': 'JavaUnitTestedApp'
  };

  appType: string;
  problem: string;
  namePattern$: Observable<string[]>;
  private searchTerms = new Subject<string>();
  isProblemSelected = false;
  isProblemTypeSelected = false;

  constructor(
    private problemService: ProblemService,
  ) {
  }

  ngOnInit(): void {
    this.namePattern$ = this.searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) =>
        this.problemService.getProblemsByName(this.appType, term)
          .toPromise()
          .catch(err => [err.message]))
    );
  }

  async onSubmit($event: Event, submitBtn: HTMLButtonElement): Promise<void> {
    $event.preventDefault();
    submitBtn.disabled = true;

    await this.problemService.delete(this.appType, this.problem);
    submitBtn.disabled = false;
  }

  search(term: string): void {
    this.isProblemSelected = false;
    this.searchTerms.next(term);
  }

  onSelectProblem(problemName: string): void {
    this.problem = problemName;
    this.isProblemSelected = true;
  }

  onProblemTypeSelect(): void {
    this.isProblemTypeSelected = true;
  }
}
