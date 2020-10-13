import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProblemService {
  SERVER_URL = 'https://graderbot.herokuapp.com/Problems';

  constructor(
    private httpClient: HttpClient
  ) {
  }

  addNew(appType: string, problemName: string, problemDefinition: File): Promise<void> {
    const formData = new FormData();
    formData.append('name', problemName);
    formData.append('problemFiles', problemDefinition);

    return this.httpClient
      .post(`${this.SERVER_URL}/${appType}/AddNew`, formData)
      .toPromise()
      .catch(e => e.toString());
  }

  delete(appType: string, problemName: string): Promise<void>{
    return this.httpClient
      .post(`${this.SERVER_URL}/${appType}/Delete/${problemName}`, null)
      .toPromise()
      .catch(err => err.toString());
  }

  getTaskDescription(appType: string, problem: string): Observable<string> {
    return this.httpClient
      .get(`${this.SERVER_URL}/${appType}/Description/${problem}`, {responseType: 'text'});
  }

  getProblemsByName(appType: string, term: string): Observable<string[]> {
    return this.httpClient
      .get<string[]>(`${this.SERVER_URL}/${appType}/ListAll/${encodeURIComponent(term || '$^')}`);
  }
}
