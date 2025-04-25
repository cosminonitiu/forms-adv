import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConditionalVisibilityDialogComponent } from './conditional-visibility-dialog.component';

describe('ConditionalVisibilityDialogComponent', () => {
  let component: ConditionalVisibilityDialogComponent;
  let fixture: ComponentFixture<ConditionalVisibilityDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConditionalVisibilityDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConditionalVisibilityDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
