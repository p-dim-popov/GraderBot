import {Injectable, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {BehaviorSubject, Observable, pipe, Subject} from 'rxjs';
import {SolutionDto} from './solution-dto';
import {multicast, refCount} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class SolutionService {
  SERVER_URL = 'https://localhost:44347/Problems';
  solution$ = new Subject<SolutionDto>();

  constructor(
    private httpClient: HttpClient
  ) {
  }

  async submitSolution(appType: string, problem: string, solution: File): Promise<void> {
    const formData = new FormData();
    formData.append('problemSolution', solution);
    this.solution$.next(await this.httpClient
      .post<SolutionDto>(`${this.SERVER_URL}/${appType}/Submit/${problem}`, formData)
      .toPromise());
  }

  getTaskDescription(appType: string, problem: string): Observable<string> {
    return this.httpClient
      .get(`${this.SERVER_URL}/${appType}/TaskDescription/${problem}`, {responseType: 'text'});
  }

  getProblemsByName(appType: string, term: string): Observable<string[]> {
    return this.httpClient
      .get<string[]>(`${this.SERVER_URL}/${appType}/ListAll/${encodeURIComponent(term || '$^')}`);
  }
}