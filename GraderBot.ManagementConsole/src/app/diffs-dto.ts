import {DiffProxy} from './diff-proxy';

export class DiffsDto {
  diffs: DiffProxy[];
  hasDiffs: boolean;
  expected: string;
  actual: string;

  sideBySide(): { expectedOutput: DiffProxy[], actualOutput: DiffProxy[]} {
    const expectedOutput = this.diffs
      .filter(d => d.operation === 'E' || d.operation === 'D');

    const actualOutput = this.diffs
      .filter(d => d.operation === 'E' || d.operation === 'I');

    return {expectedOutput, actualOutput};
  }
}
