import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestsService } from '../../../services/entities/requests.service';
import { ConditionalVisibility, FormQuestion, FormRequest, FormSection } from '../../../shared/models/form-request';
import { Circle, Square } from 'lucide-angular';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { SnackbarHelperService } from '../../../services/shared/snackbar-helper.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ConditionalVisibilityDialogComponent } from './conditional-visibility-dialog/conditional-visibility-dialog.component';
import { UserStore } from '../../../services/stores/user.store';
import { RequestsStore } from '../../../services/stores/requests.store';

@Component({
  selector: 'app-new-request',
  standalone: false,
  templateUrl: './new-request.component.html',
  styleUrl: './new-request.component.scss'
})
export class NewRequestComponent {
  constructor(
    private route: ActivatedRoute, 
    private router: Router,
    private userStore: UserStore,
    private requestsService: RequestsService,
    private snackbar: SnackbarHelperService,
    private dialog: MatDialog,
    private requestsStore: RequestsStore,
    public dialogRef: MatDialogRef<NewRequestComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { request: FormRequest}
  ) {}

  public readonly Circle = Circle;
  public readonly Square = Square;

  public questionTypes = ['SingleChoice', 'MultipleChoice', 'Text', 'YesNo', 'Date', 'Dropdown' , 'AD', 'Number'];


  addSection() {
    if(this.data.request) {
      const newSection: FormSection = {
        id: this.generateGUID(),
        name: "New Section",
        questions: []
      }
      this.data.request.sections.push(newSection);
    }
  }

  addQuestion(sectionIndex: number) {
    if(this.data.request) {
      const newQuestion: FormQuestion = {
        id: this.generateGUID(),
        text: '',
        required: false,
        type: 'SingleChoice',
        options: [],
        maxAnswer: 0,
        minAnswer: 0,
        conditionalVisibilities: []
      };
      this.data.request.sections[sectionIndex].questions.push(newQuestion);
    }
  }

  removeQuestion(sectionIndex: number, index: number) {
    if(this.data.request) {
      this.data.request.sections[sectionIndex].questions.splice(index, 1);
    } 
  }

  addOption(question: FormQuestion) {
    question.options.push('');
  }

  removeOption(question: FormQuestion, index: number) {
    question.options.splice(index, 1);
  }

  saveForm() {
    if(this.data.request) {
      this.requestsService.apiFormRequestsPut(this.data.request).subscribe(() => {
        this.requestsStore.reloadOwnedRequests();
        this.requestsStore.reloadFormRequests();
        this.onNoClick();
      })
    }
  }

  onNoClick(): void {
    this.dialogRef.close('CLOSE');
  }
  public drop(event: CdkDragDrop<string[]>, sectionIndex: number) {
    if(this.data.request){
      if(event.previousContainer === event.container) {
        moveItemInArray(this.data.request.sections[sectionIndex].questions, event.previousIndex, event.currentIndex);
      } else {
        try {
          const previousSectionIndex = Number(event.previousContainer.id.split("-")[1])
          transferArrayItem(
            this.data.request.sections[previousSectionIndex].questions,
            this.data.request.sections[sectionIndex].questions,
            event.previousIndex,
            event.currentIndex
          );
        } catch {
          this.snackbar.createErrorNotification("Could not parse category index when moving");
        }  
      }
    }
  }
  public moveUp(sectionIndex: number, previousIndex: number) {
    if(this.data.request){
      moveItemInArray(this.data.request.sections[sectionIndex].questions, previousIndex, previousIndex - 1);
    }
  }
  public moveDown(sectionIndex: number, previousIndex: number) {
    if(this.data.request){
      moveItemInArray(this.data.request.sections[sectionIndex].questions, previousIndex, previousIndex + 1);
    } 
  }

  get connectedDropLists() {
    if(this.data.request) {
      return this.data.request.sections.map((_, idx) => `category-${idx}`);
    } else {
      return [];
    } 
  }

  get connectedNavDropLists() {
    if(this.data.request) {
      return this.data.request.sections.map((_, idx) => `nav-category-${idx}`);
    } else {
      return [];
    } 
  }

  scrollToSection(sectionIndex: number) {
    const element = document.getElementById(`category-${sectionIndex}`);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  scrollToQuestion(sectionIndex: number, questionIndex: number) {
    const element = document.getElementById(`question-${sectionIndex}-${questionIndex}`);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  public onViewConditionalVisibility(question: FormQuestion) {
    if(this.data.request) {
      const dialogRef = this.dialog.open(ConditionalVisibilityDialogComponent, {
        panelClass: 'dialog-box',
        data: { question: question, request: this.data.request }
      })
      dialogRef.afterClosed().subscribe((data: string | ConditionalVisibility[]) => {
        if(data !== 'CLOSE') {
          question.conditionalVisibilities = data as ConditionalVisibility[];
        }
      })
    }
  }

  public generateGUID(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c: string): string {
      const r = Math.random() * 16 | 0;
      const v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }

   // Workaround done since inputs in ngfors make them lose focus after typing
   public trackByFn(index: any, item: any) {
    return index;
  }
}

