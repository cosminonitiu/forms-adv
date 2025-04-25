import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestsService } from '../../../services/entities/requests.service';
import { SnackbarHelperService } from '../../../services/shared/snackbar-helper.service';
import { FormRequest, FormSection, FormQuestion } from '../../../shared/models/form-request';
import { SubmittedRequestsService } from '../../../services/entities/submitted-requests.service';
import { SubmittedConditionalVisibility, SubmittedRequest, SubmittedRequestQuestion, SubmittedRequestSection } from '../../../shared/models/submitted-requests';
import { ADService } from '../../../services/entities/ad.service';
import { ADUser } from '../../../shared/models/ad-user';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-approve-request',
  standalone: false,
  templateUrl: './approve-request.component.html',
  styleUrl: './approve-request.component.scss'
})
export class ApproveRequestComponent {

  constructor(
    private adService: ADService,
    private submittedRequestsService: SubmittedRequestsService,
    private snackbar: SnackbarHelperService,
    public dialogRef: MatDialogRef<ApproveRequestComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { viewOnly: boolean, submittedRequest: SubmittedRequest  }
  ) {
  
  }
  
  public ADOptions: ADUser[] = [];

  ngOnInit(): void {
    if(this.data.submittedRequest) {
      this.data.submittedRequest.sections.forEach((s: SubmittedRequestSection) => {
        s.questions.forEach((q: SubmittedRequestQuestion) => {
          q.visible = true;
        })
      })
      let adquestionpresent = false;
      this.data.submittedRequest.sections.forEach((s: SubmittedRequestSection) => {
        s.questions.forEach((q: SubmittedRequestQuestion) => {
          q.conditionalVisibilityTriggerForOtherQuestion.forEach((c: SubmittedConditionalVisibility) => {
            const foundSection = this.data.submittedRequest?.sections.find(s => s.sectionId === c.sectionId);
            if(foundSection) {
              const foundQuestion = foundSection.questions.find(q => q.questionId == c.questionId);
              if(foundQuestion) {
                if(foundQuestion.type === 'MultipleChoice') {
                  if(c.type === 'Equals') {
                    foundQuestion.visible = q.answers.includes(c.option);
                  } else if(c.type === 'NotEquals') {
                    foundQuestion.visible = !q.answers.includes(c.option);
                  } else if(c.type === 'Contains') {
                    let visible = false;
                    c.options.forEach((opt: string) => {
                      if(q.answers.includes(opt)) {
                        visible = true;
                      }
                    })
                    foundQuestion.visible = visible;
                  } else if(c.type === 'NotContains') {
                    let visible = true;
                    c.options.forEach((opt: string) => {
                      if(q.answers.includes(opt)) {
                        visible = false;
                      }
                    })
                    foundQuestion.visible = visible;
                  }
                } else {
                  if(c.type === 'Equals') {
                    foundQuestion.visible = q.answer === c.option ? true : false;
                  } else if(c.type === 'NotEquals') {
                    foundQuestion.visible = q.answer === c.option ? false : true;
                  } else if(c.type === 'Contains') {
                    foundQuestion.visible = c.options.includes(q.answer);
                  } else if(c.type === 'NotContains') {
                    foundQuestion.visible = !c.options.includes(q.answer);
                  }
                }
              } else {
                console.log("Could not determine Question to Attach Visiblity Trigger " + c.questionId)
              }
            } else {
              console.log("Could not determine Section to Attach Visiblity Trigger " + c.sectionId)
            }
          })

          if(q.type === 'AD' && adquestionpresent === false) {
            adquestionpresent = true;
            this.adService.apiAllUsersGet().subscribe((data: ADUser[]) => {
              this.ADOptions = data;
            })
          }
          if(q.type === 'YesNo') {
            if(q.answer.toLowerCase() === 'yes') {
              q.booleanAnswer = true;
            } else {
              q.booleanAnswer = false;
            }
          }
        })
      })
    }
  }


  currentSectionIndex = 0;

  get currentSection(): SubmittedRequestSection | null {
    if(this.data.submittedRequest) {
      return this.data.submittedRequest.sections[this.currentSectionIndex];
    }
    return null
  }

  get isFirstSection(): boolean {
    return this.currentSectionIndex === 0;
  }

  get isLastSection(): boolean {
    if(this.data.submittedRequest) {
      return this.currentSectionIndex === this.data.submittedRequest.sections.length - 1;
    }
    return false
  }

  nextSection(): void {
    if (!this.isLastSection) {
      this.currentSectionIndex++;
    }
  }

  previousSection(): void {
    if (!this.isFirstSection) {
      this.currentSectionIndex--;
    }
  }

  public approve() {
    this.submittedRequestsService.apiSubmittedRequestsApprove(this.data.submittedRequest.id, this.data.submittedRequest.owner).subscribe(() => {
      this.dialogRef.close("DONE")
    })
  }

  public reject() {
    this.submittedRequestsService.apiSubmittedRequestsReject(this.data.submittedRequest.id, this.data.submittedRequest.owner).subscribe(() => {
      this.dialogRef.close("DONE")
    })
  }

  onNoClick(): void {
    this.dialogRef.close('CLOSE');
  }
}
