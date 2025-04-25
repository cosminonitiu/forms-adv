import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestsService } from '../../services/entities/requests.service';
import { ConditionalVisibility, FormQuestion, FormRequest, FormSection } from '../../shared/models/form-request';
import { Circle, Square } from 'lucide-angular';
import { CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
import { SnackbarHelperService } from '../../services/shared/snackbar-helper.service';
import { MatDialog } from '@angular/material/dialog';
import { ConditionalVisibilityDialogComponent } from './conditional-visibility-dialog/conditional-visibility-dialog.component';
import { UserStore } from '../../services/stores/user.store';

@Component({
  selector: 'app-new-request',
  standalone: false,
  templateUrl: './new-request.component.html',
  styleUrl: './new-request.component.scss'
})
export class NewRequestComponent implements OnInit{
  constructor(
    private route: ActivatedRoute, 
    private router: Router,
    private userStore: UserStore,
    private requestsService: RequestsService,
    private snackbar: SnackbarHelperService,
    private dialog: MatDialog
  ) {
    this.route.params.subscribe(params => {
      this.requestId = params['requestId'];
    })
  }

  public readonly Circle = Circle;
  public readonly Square = Square;

  public requestId: string | null = null;
  public request: FormRequest | null = null; 

  public questionTypes = ['SingleChoice', 'MultipleChoice', 'Text', 'YesNo', 'Date', 'Dropdown' , 'AD'];

  ngOnInit(): void {
    const userId = this.userStore.currentAccount?.localAccountId;
    if(userId) {
      if(this.requestId != '' && this.requestId != null) {
        this.requestsService.apiFormRequestGet(this.requestId, userId).subscribe((data: FormRequest | null) => {
          this.request = data;
        })
      }
    }  
  }


  addSection() {
    if(this.request) {
      const newSection: FormSection = {
        id: this.generateGUID(),
        name: "New Section",
        questions: []
      }
      this.request.sections.push(newSection);
    }
  }

  addQuestion(sectionIndex: number) {
    if(this.request) {
      const newQuestion: FormQuestion = {
        id: this.generateGUID(),
        text: '',
        required: false,
        type: 'SingleChoice',
        options: [],
        conditionalVisibilities: []
      };
      this.request.sections[sectionIndex].questions.push(newQuestion);
    }
  }

  removeQuestion(sectionIndex: number, index: number) {
    if(this.request) {
      this.request.sections[sectionIndex].questions.splice(index, 1);
    } 
  }

  addOption(question: FormQuestion) {
    question.options.push('');
  }

  removeOption(question: FormQuestion, index: number) {
    question.options.splice(index, 1);
  }

  saveForm() {
    if(this.request) {
      this.requestsService.apiFormRequestsPut(this.request).subscribe(() => {
        this.navigateBack();
      })
    }
  }

  navigateBack() {
    //this.router.navigate(['/manage']) //TODO
  }
  public drop(event: CdkDragDrop<string[]>, sectionIndex: number) {
    if(this.request){
      if(event.previousContainer === event.container) {
        moveItemInArray(this.request.sections[sectionIndex].questions, event.previousIndex, event.currentIndex);
      } else {
        try {
          const previousSectionIndex = Number(event.previousContainer.id.split("-")[1])
          transferArrayItem(
            this.request.sections[previousSectionIndex].questions,
            this.request.sections[sectionIndex].questions,
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
    if(this.request){
      moveItemInArray(this.request.sections[sectionIndex].questions, previousIndex, previousIndex - 1);
    }
  }
  public moveDown(sectionIndex: number, previousIndex: number) {
    if(this.request){
      moveItemInArray(this.request.sections[sectionIndex].questions, previousIndex, previousIndex + 1);
    } 
  }

  get connectedDropLists() {
    if(this.request) {
      return this.request.sections.map((_, idx) => `category-${idx}`);
    } else {
      return [];
    } 
  }

  get connectedNavDropLists() {
    if(this.request) {
      return this.request.sections.map((_, idx) => `nav-category-${idx}`);
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
    if(this.request) {
      const dialogRef = this.dialog.open(ConditionalVisibilityDialogComponent, {
        panelClass: 'dialog-box',
        data: { question: question, request: this.request }
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
}

