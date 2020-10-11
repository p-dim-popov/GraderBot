import {Component, OnInit} from '@angular/core';
import {SolutionDto} from './solution-dto';
import {SolutionService} from './solution.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'GraderBotManagementConsole';
  solution: SolutionDto;

  constructor(
    private solutionService: SolutionService
  ) {
  }

  ngOnInit(): void {
    this.solutionService.solution$
      .subscribe((s) => {
          this.solution = s;
        }
      );
  }
}
