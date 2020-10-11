import {Component, OnInit} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
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

  problemDescription: string;
  appType: string;
  problem: string;
  namePattern$: Observable<string[]>;
  private searchTerms = new Subject<string>();
  private solutionFile: File;
  isProblemSelected = false;

  constructor(
    private solutionService: SolutionService,
  ) {
  }

  ngOnInit(): void {
    this.namePattern$ = this.searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) =>
        this.solutionService.getProblemsByName(this.appType, term)
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
    this.solutionService.getTaskDescription(this.appType, this.problem)
      .subscribe(td => {
        this.problemDescription = td;
        showProblemDescriptionBtn.disabled = false;
      });
  }

  onProblemTypeSelect(problemInput: HTMLInputElement): void {
    problemInput.disabled = false;
  }
}
