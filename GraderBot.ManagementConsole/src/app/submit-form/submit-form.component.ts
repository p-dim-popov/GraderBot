import {Component, OnInit} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, switchMap} from 'rxjs/operators';
import {HttpClient} from '@angular/common/http';
import {DiffsDto} from '../diffs-dto';
import { DiffContent, DiffResults } from 'ngx-text-diff/lib/ngx-text-diff.model';

@Component({
  selector: 'app-submit-form',
  templateUrl: './submit-form.component.html',
  styleUrls: ['./submit-form.component.css']
})
export class SubmitFormComponent implements OnInit {
  SERVER_URL = 'https://localhost:44347/Problems/JavaConsoleApp';

  problem: string;
  problemNames$: Observable<string[]>;
  private searchTerms = new Subject<string>();
  private solutionFile: File;
  isProblemSelected = false;
  diffs: DiffsDto[];

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
          .get<string[]>(`${this.SERVER_URL}/ListAll/${encodeURIComponent(term || '$^')}`)
          .toPromise()
          .catch(err => [err.message]))
    );
  }

  onSubmit($event: Event): void {
    $event.preventDefault();

    const formData = new FormData();
    formData.append('problemSolution', this.solutionFile);

    this.httpClient.post<DiffsDto[]>(`${this.SERVER_URL}/Submit/${this.problem}`, formData)
      .subscribe(
        (res) => this.diffs = res,
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
}
