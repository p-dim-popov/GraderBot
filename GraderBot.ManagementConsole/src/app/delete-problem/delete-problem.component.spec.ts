import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteProblemComponent } from './delete-problem.component';

describe('DeleteProblemComponent', () => {
  let component: DeleteProblemComponent;
  let fixture: ComponentFixture<DeleteProblemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteProblemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteProblemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
