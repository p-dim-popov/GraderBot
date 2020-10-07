import {Component, OnInit} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';
import {OutputsDto} from '../outputs-dto';
import {DiffContent, DiffResults} from 'ngx-text-diff/lib/ngx-text-diff.model';
import {SolutionDto} from '../solution-dto';

@Component({
  selector: 'app-submit-form',
  templateUrl: './submit-form.component.html',
  styleUrls: ['./submit-form.component.css']
})
export class SubmitFormComponent implements OnInit {
  SERVER_URL = 'https://localhost:44347/Problems';

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
  solution: SolutionDto;

  constructor(
    private httpClient: HttpClient
  ) {
  }

  ngOnInit(): void {
    this.problemNames$ = this.searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) =>
        this.httpClient
          .get<string[]>(`${this.SERVER_URL}/${this.appType}/ListAll/${encodeURIComponent(term || '$^')}`)
          .toPromise()
          .catch(err => [err.message]))
    );
  }

  onSubmit($event: Event, submitBtn: HTMLButtonElement): void {
    $event.preventDefault();
    submitBtn.disabled = true;

    const formData = new FormData();
    formData.append('problemSolution', this.solutionFile);

    this.httpClient.post<SolutionDto>(`${this.SERVER_URL}/${this.appType}/Submit/${this.problem}`, formData)
      .subscribe(
        (res) => {
          this.solution = res;
          submitBtn.disabled = false;
        },
        (err) => console.log(err)
      );
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

  onFocusOut(): void {
    this.isProblemSelected = true;
  }

  onGetTask(): void {
    this.httpClient
      .get(`${this.SERVER_URL}/${this.appType}/TaskDescription/${this.problem}`, {responseType: 'text'})
      .subscribe(s => this.taskDescription = s);
  }
}
