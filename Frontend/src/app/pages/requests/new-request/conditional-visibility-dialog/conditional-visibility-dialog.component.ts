import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConditionalVisibility, FormQuestion, FormQuestionNoConditionals, FormRequest, FormSection } from '../../../../shared/models/form-request';
import { SnackbarHelperService } from '../../../../services/shared/snackbar-helper.service';

@Component({
  selector: 'app-conditional-visibility-dialog',
  standalone: false,
  templateUrl: './conditional-visibility-dialog.component.html',
  styleUrl: './conditional-visibility-dialog.component.scss'
})
export class ConditionalVisibilityDialogComponent implements OnInit{
  constructor(
    public dialogRef: MatDialogRef<ConditionalVisibilityDialogComponent>,
    private snackbar: SnackbarHelperService,
    @Inject(MAT_DIALOG_DATA)
    public data: { question: FormQuestion, request: FormRequest }
  ){}

  public currentConditions: ConditionalVisibility[] = [];
  public possibleTypes = ['Equals' , 'NotEquals' , 'Contains' , 'NotContains'];

  ngOnInit(): void {
    if(this.data.request && this.data.question) {
      this.currentConditions = structuredClone(this.data.question.conditionalVisibilities);
      this.currentConditions.forEach((cond: ConditionalVisibility) => {
        const section = this.data.request.sections.find(s => s.id == cond.sectionId);
        if(section !== null && section !== undefined) {
          cond.possibleQuestions = section.questions.map((q) => {
            const question: FormQuestionNoConditionals = {
              id: q.id,
              type: q.type,
              options: q.options,
              text: q.text
            }
            return question;
          })
        } else {
          cond.possibleQuestions = [];
        }
      })
    }
  }

  public addCondition() {
    const firstSection = this.data.request.sections[0];
    if(firstSection) {
      const firstQuestion = firstSection.questions[0];
      if(firstQuestion) {
        const newCondition: ConditionalVisibility = {
          questionId: firstQuestion.id,
          questionText: firstQuestion.text,
          sectionId: firstSection.id,
          sectionName: firstSection.name,
          type: 'Equals',
          option: '',
          options: []
        }
        newCondition.possibleQuestions = firstSection.questions.map((q) => {
          const question: FormQuestionNoConditionals = {
            id: q.id,
            type: q.type,
            options: q.options,
            text: q.text
          }
          return question;
        })
        this.currentConditions.push(newCondition);
      }
    }
  }

  public onSectionChange(cond: ConditionalVisibility) {
    const section = this.data.request.sections.find(s => s.id == cond.sectionId);
    if(section !== null && section !== undefined) {
      cond.possibleQuestions = section.questions.map((q) => {
        const question: FormQuestionNoConditionals = {
          id: q.id,
          type: q.type,
          options: q.options,
          text: q.text
        }
        return question;
      })
    } else {
      cond.possibleQuestions = [];
    }
  }

  public addOption(cond: ConditionalVisibility) {
    cond.options.push('')
  }

  onSave(): void {
    this.dialogRef.close(this.currentConditions);
  }

  onNoClick(): void {
    this.dialogRef.close('CLOSE');
  }
}
