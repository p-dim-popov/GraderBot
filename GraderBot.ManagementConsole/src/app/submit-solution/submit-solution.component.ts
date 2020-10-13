import {Component, OnInit} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
import {SolutionService} from '../solution.service';
import {ProblemService} from '../problem.service';

@Component({
  selector: 'app-submit-form',
  templateUrl: './submit-solution.component.html',
  styleUrls: ['./submit-solution.component.css']
})
export class SubmitSolutionComponent implements OnInit {
  apps = {
    'Java Console Application': 'JavaConsoleApp',
    'Java Unit Tested Application': 'JavaUnitTestedApp'
  };

  problemDescription: string;
  appType: string;
  problem: string;
  namePattern$: Observable<string[]>;
  private searchTerms = new Subject<string>();
  private solutionFile: File;
  isProblemSelected = false;
  isProblemTypeSelected = false;

  constructor(
    private problemService: ProblemService,
    private solutionService: SolutionService,
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

    await this.solutionService.submitSolution(this.appType, this.problem, this.solutionFile);
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

  onFileSelect(target: EventTarget): void {
    const files = (target as any).files;
    if (files.length > 0) {
      this.solutionFile = files[0];
    }
  }

  onGetProblemDescription(showProblemDescriptionBtn: HTMLButtonElement): void {
    showProblemDescriptionBtn.disabled = true;
    this.problemService.getTaskDescription(this.appType, this.problem)
      .subscribe(td => {
        this.problemDescription = td;
        showProblemDescriptionBtn.disabled = false;
      });
  }

  onProblemTypeSelect(): void {
    this.isProblemTypeSelected = true;
  }
}
