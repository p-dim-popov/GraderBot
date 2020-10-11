import {Component, OnInit} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {combineAll, debounceTime, distinctUntilChanged, switchAll, switchMap} from 'rxjs/operators';
import {SolutionService} from '../solution.service';

@Component({
  selector: 'app-submit-form',
  templateUrl: './submit-form.component.html',
  styleUrls: ['./submit-form.component.css']
})
export class SubmitFormComponent implements OnInit {
  apps = {
    'Java Console Application': 'JavaConsoleApp',
    'Java Unit Tested Application': 'JavaUnitTestedApp'
  };

  taskDescription: string;
  appType: string;
  problem: string;
  problemNames$: Observable<string[]>;
  private searchTerms = new Subject<string>();
  private solutionFile: File;
  isProblemSelected = false;

  constructor(
    private solutionService: SolutionService,
  ) {
  }

  ngOnInit(): void {
    this.problemNames$ = this.searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) =>
        this.solutionService.getProblemsByName(this.appType, term)
          .toPromise()
          .catch(err => [err.message]))
    );
  }

  onSubmit($event: Event, submitBtn: HTMLButtonElement): void {
    $event.preventDefault();
    // submitBtn.disabled = true;

    this.solutionService.submitSolution(this.appType, this.problem, this.solutionFile);
  }

  search(term: string): void {
    this.isProblemSelected = false;
    this.searchTerms.next(term);
  }

  onSelectProblem(problemName: string): void {
    this.problem = problemName;
    this.isProblemSelected = true;
  }

  onFileSelect(target: EventTarget): void {
    const files = (target as any).files;
    if (files.length > 0) {
      this.solutionFile = files[0];
    }
  }

  onGetTask(): void {
    this.solutionService.getTaskDescription(this.appType, this.problem)
      .subscribe(td => this.taskDescription = td);
  }
}
