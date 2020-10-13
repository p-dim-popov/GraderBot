import {Component, OnInit} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {SolutionService} from '../solution.service';
import {debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
import {ProblemService} from '../problem.service';

@Component({
  selector: 'app-add-new',
  templateUrl: './add-new-problem.component.html',
  styleUrls: ['./add-new-problem.component.css']
})
export class AddNewProblemComponent implements OnInit {

  apps = {
    'Java Console Application': 'JavaConsoleApp',
    'Java Unit Tested Application': 'JavaUnitTestedApp'
  };

  appType: string;
  problem: string;
  namePattern$: Observable<string[]>;
  private searchTerms = new Subject<string>();
  private problemDefinition: File;
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

  async onSubmit($event: Event, addNewBtn: HTMLButtonElement): Promise<void> {
    $event.preventDefault();
    addNewBtn.disabled = true;

    await this.problemService.addNew(this.appType, this.problem, this.problemDefinition);
    addNewBtn.disabled = false;
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
      this.problemDefinition = files[0];
    }
  }

  onProblemTypeSelect(): void {
    this.isProblemTypeSelected = true;
  }
}
