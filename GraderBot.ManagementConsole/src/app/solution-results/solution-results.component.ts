import {Component, Input, OnInit} from '@angular/core';
import {OutputsDto} from '../outputs-dto';

@Component({
  selector: 'app-solution-results',
  templateUrl: './solution-results.component.html',
  styleUrls: ['./solution-results.component.css']
})
export class SolutionResultsComponent implements OnInit {
  @Input() outputs: OutputsDto[];
  @Input() id: string;

  constructor() { }

  ngOnInit(): void {
  }

}
