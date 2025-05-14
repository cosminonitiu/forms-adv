import { Component, Inject, OnInit } from '@angular/core';
import { ConditionalVisibility, FormQuestion, FormRequest, FormSection } from '../../../shared/models/form-request';
import { ActivatedRoute, Router } from '@angular/router';
import { RequestsService } from '../../../services/entities/requests.service';
import { SnackbarHelperService } from '../../../services/shared/snackbar-helper.service';
import { SubmittedConditionalVisibility, SubmittedRequest, SubmittedRequestQuestion, SubmittedRequestSaveDraft, SubmittedRequestSection, SubmittedRequestUpdateModel } from '../../../shared/models/submitted-requests';
import { SubmittedRequestsService } from '../../../services/entities/submitted-requests.service';
import { MsalService } from '@azure/msal-angular';
import { AccountInfo } from '@azure/msal-browser';
import { HttpClient } from '@angular/common/http';
import { ADService } from '../../../services/entities/ad.service';
import { ADUser } from '../../../shared/models/ad-user';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserStore } from '../../../services/stores/user.store';
import { RequestsStore } from '../../../services/stores/requests.store';

@Component({
  selector: 'app-submit-request',
  standalone: false,
  templateUrl: './submit-request.component.html',
  styleUrl: './submit-request.component.scss'
})
export class SubmitRequestComponent implements OnInit{
  constructor(
    private route: ActivatedRoute, 
    private router: Router,
    private requestsService: RequestsService,
    private userStore: UserStore,
    private http: HttpClient,
    private adService: ADService,
    private submittedRequestsService: SubmittedRequestsService,
    private snackbar: SnackbarHelperService,
    private requestsStore: RequestsStore,
    public dialogRef: MatDialogRef<SubmitRequestComponent>,
    @Inject(MAT_DIALOG_DATA)
    public data: { draft: boolean, request?: FormRequest, submittedRequest?: SubmittedRequest  }
  ) {}
  
  public ADOptions: ADUser[] = [];
  
  public formToSubmit: SubmittedRequest | null = null;
  public currentUser: AccountInfo | null = null;

  ngOnInit(): void {
    let activeAccount = this.userStore.currentAccount;
    if(activeAccount && activeAccount.localAccountId) {
      this.currentUser = activeAccount;
      if(this.data.draft === true && this.data.submittedRequest !== null && this.data.submittedRequest !== undefined) {
        this.loadDraft(this.data.submittedRequest);
      } else if(this.data.draft === false && this.data.request !== null && this.data.request !== undefined) {
        this.loadRequestData(this.data.request);
      }
    }
  }

  private loadDraft(request: SubmittedRequest) {
    this.formToSubmit = request;

    this.formToSubmit.sections.forEach((s: SubmittedRequestSection) => {
      s.questions.forEach((q: SubmittedRequestQuestion) => {
        q.visible = true;
      })
    })

    // Conditional AD Options load and visiblity
    let adquestionpresent = false;
    this.formToSubmit.sections.forEach((s: SubmittedRequestSection) => {
      s.questions.forEach((q: SubmittedRequestQuestion) => {
        q.conditionalVisibilityTriggerForOtherQuestion.forEach((c: SubmittedConditionalVisibility) => {
          const foundSection = this.formToSubmit?.sections.find(s => s.sectionId === c.sectionId);
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
              } else if(foundQuestion.type === 'Number') {
                if(c.type === 'Equals') {
                  foundQuestion.visible = parseInt(q.answer) === c.numberOption ? true : false;
                } else if(c.type === 'NotEquals') {
                  foundQuestion.visible = parseInt(q.answer) === c.numberOption ? false : true;
                } else if(c.type === 'Contains') {
                  foundQuestion.visible = c.numberOptions.includes(parseInt(q.answer));
                } else if(c.type === 'NotContains') {
                  foundQuestion.visible = !c.numberOptions.includes(parseInt(q.answer));
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
      s.requiredDone = this.isSectionRequiredDone(s);
    })
  }

  private loadRequestData(request: FormRequest) {
    if(this.currentUser && this.currentUser.name && this.currentUser.localAccountId) {
      // Load Request Data
      this.formToSubmit = {
        id: '',
        formId: request.id,
        formName: request.name,
        approverName: '',
        approverUID: '',
        ownerName: this.currentUser.name,
        owner: this.currentUser.localAccountId,
        state: 'Draft',
        created: new Date().toUTCString(),
        hideSections: request.hideSections,
        description: request.description,
        icon: request.icon,
        color: request.color,
        sections: []
      }
      request.sections.forEach((s: FormSection) => {
        let newSection: SubmittedRequestSection = {
          sectionId: s.id,
          name: s.name,
          questions: [] 
        };
        s.questions.forEach((q: FormQuestion) => {
          let newQuestion: SubmittedRequestQuestion = {
            questionId: q.id,
            text: q.text,
            type: q.type,
            answer: '',
            required: q.required,
            answers: [],
            options: q.options,
            conditionalVisibilityTriggerForOtherQuestion:[],
            visible: true,
            maxAnswer: q.maxAnswer,
            minAnswer: q.minAnswer
          }
          q.conditionalVisibilities.forEach((c: ConditionalVisibility) => {
            if(c.type === 'Contains' || c.type === 'Equals') {
              newQuestion.visible = false;
            }
          })
          if(q.type === 'YesNo') {
            newQuestion.booleanAnswer = false;
            newQuestion.answer = 'No';
          }
          newSection.questions.push(newQuestion);
        })
        newSection.requiredDone = this.isSectionRequiredDone(newSection);
        this.formToSubmit?.sections.push(newSection);
      })

      // Attach Triggers for Visiblity to Questions and Required Done
      request.sections.forEach((s: FormSection) => {
        s.questions.forEach((q: FormQuestion) => {
          q.conditionalVisibilities.forEach((c: ConditionalVisibility) => {
            const foundSection = this.formToSubmit?.sections.find(s => s.sectionId == c.sectionId);
            if(foundSection) {
              const foundQuestion = foundSection.questions.find(q => q.questionId == c.questionId);
              if(foundQuestion) {
                const newConditions: SubmittedConditionalVisibility = {
                  sectionId: s.id,
                  sectionName: s.name,
                  questionId: q.id,
                  questionText: q.text,
                  type: c.type,
                  option: c.option,
                  numberOption: c.numberOption,
                  options: c.options,
                  numberOptions: c.numberOptions
                }
                foundQuestion.conditionalVisibilityTriggerForOtherQuestion.push(newConditions);
              } else {
                console.log("Could not determine Question to Attach Visiblity Trigger " + c.questionId)
              }
            } else {
              console.log("Could not determine Section to Attach Visiblity Trigger " + c.sectionId)
            }
          })
        })
      })

      // Conditional AD Options load 
      let adquestionpresent = false;
      request.sections.forEach((s: FormSection) => {
        s.questions.forEach((q: FormQuestion) => {
          
          if(q.type === 'AD' && adquestionpresent === false) {
            adquestionpresent = true;
            this.adService.apiAllUsersGet().subscribe((data: ADUser[]) => {
              this.ADOptions = data;
            })
          }
        })
      })
    }
  }

  public onAnswerChange(question: SubmittedRequestQuestion) {
    if(question.conditionalVisibilityTriggerForOtherQuestion.length > 0) {
      question.conditionalVisibilityTriggerForOtherQuestion.forEach(c => {
        const foundSection = this.formToSubmit?.sections.find(s => s.sectionId == c.sectionId);
        if(foundSection) {
          const foundQuestion = foundSection.questions.find(q => q.questionId == c.questionId);
          if(foundQuestion) {
            if(question.type === 'MultipleChoice') {
              if(c.type === 'Equals') {
                foundQuestion.visible = question.answers.includes(c.option);
              } else if(c.type === 'NotEquals') {
                foundQuestion.visible = !question.answers.includes(c.option);
              } else if(c.type === 'Contains') {
                let visible = false;
                c.options.forEach((opt: string) => {
                  if(question.answers.includes(opt)) {
                    visible = true;
                  }
                })
                foundQuestion.visible = visible;
              } else if(c.type === 'NotContains') {
                let visible = true;
                c.options.forEach((opt: string) => {
                  if(question.answers.includes(opt)) {
                    visible = false;
                  }
                })
                foundQuestion.visible = visible;
              }
            } else if(question.type === 'Number') {
              if(c.type === 'Equals') {
                foundQuestion.visible = parseInt(question.answer) === c.numberOption ? true : false;
              } else if(c.type === 'NotEquals') {
                foundQuestion.visible = parseInt(question.answer) === c.numberOption ? false : true;
              } else if(c.type === 'Contains') {
                foundQuestion.visible = c.numberOptions.includes(parseInt(question.answer));
              } else if(c.type === 'NotContains') {
                foundQuestion.visible = !c.numberOptions.includes(parseInt(question.answer));
              }
            } else {
              if(c.type === 'Equals') {
                foundQuestion.visible = question.answer === c.option ? true : false;
              } else if(c.type === 'NotEquals') {
                foundQuestion.visible = question.answer === c.option ? false : true;
              } else if(c.type === 'Contains') {
                foundQuestion.visible = c.options.includes(question.answer);
              } else if(c.type === 'NotContains') {
                foundQuestion.visible = !c.options.includes(question.answer);
              }
            }
          } else {
            console.log("Could not determine question to calculate visiblity " + c.questionId)
          }
        } else {
          console.log("Could not determine section to calculate visiblity " + c.sectionId)
        }
      })
    }
    if(question.required === true && this.currentSection) {
      this.currentSection.requiredDone = this.isSectionRequiredDone(this.currentSection);
    }
  }

  public onYesNoAnswerChange(question: SubmittedRequestQuestion) {
    if(question.booleanAnswer === true){
      question.answer = 'Yes';
    } else {
      question.answer = 'No';
    }
    this.onAnswerChange(question);
  }

  public onNumberAnswerChange(question: SubmittedRequestQuestion) {
    if(parseInt(question.answer) < question.minAnswer) {
      question.answer = question.minAnswer.toString();
    } else if(parseInt(question.answer) > question.maxAnswer) {
      question.answer = question.maxAnswer.toString();
    }
    this.onAnswerChange(question);
  }

  currentSectionIndex = 0;

  get currentSection(): SubmittedRequestSection | null {
    if(this.formToSubmit) {
      return this.formToSubmit.sections[this.currentSectionIndex];
    }
    return null
  }

  get isFirstSection(): boolean {
    return this.currentSectionIndex === 0;
  }

  get isLastSection(): boolean {
    if(this.formToSubmit) {
      return this.currentSectionIndex === this.formToSubmit.sections.length - 1;
    }
    return false
  }

  nextSection(): void {
    if(this.currentSection) {
      const requiredDone = this.isSectionRequiredDone(this.currentSection);
      if(requiredDone === true){
        if (!this.isLastSection) {
          this.currentSectionIndex++;
        }
      } else {
        this.snackbar.createErrorNotification("Required questions have to be completed")
      }
    } 
  }

  previousSection(): void {
    if (!this.isFirstSection) {
      this.currentSectionIndex--;
    }
  }

  public submit() {
    if(this.currentSection) {
      const requiredDone = this.isSectionRequiredDone(this.currentSection);
    if(requiredDone === true){
      if(this.data.draft === false && this.formToSubmit && this.data.request) {
        const model: SubmittedRequestSaveDraft = {
          formId: this.data.request.id,
          formOwnerId: this.data.request.owner,
          sections: this.formToSubmit.sections
        }
        this.submittedRequestsService.apiSaveDraftRequestsPost(model).subscribe((id: string) => {
          this.submittedRequestsService.apiSubmittedRequestsSubmit(id).subscribe(() => {
            this.requestsStore.reloadSubmittedRequests(this.data.request?.owner);
            this.onNoClick();
          })
        })
      } else if(this.data.draft === true && this.formToSubmit){
        const model: SubmittedRequestUpdateModel = {
          id: this.formToSubmit.id,
          sections: this.formToSubmit.sections
        }
        this.submittedRequestsService.apiSubmittedRequestsUpdate(model).subscribe((id: string) => {
          this.submittedRequestsService.apiSubmittedRequestsSubmit(model.id).subscribe(() => {
            this.requestsStore.reloadSubmittedRequests(this.data.request?.owner);
            this.dialogRef.close('SUBMIT');
          })
        })
      }
    } else {
      this.snackbar.createErrorNotification("Required questions have to be completed")
    } 
    }  
  }

  public saveAsDraft() {
    if(this.formToSubmit && this.data.request){
      const model: SubmittedRequestSaveDraft = {
        formId: this.data.request.id,
        formOwnerId: this.data.request.owner,
        sections: this.formToSubmit.sections
      }
      this.submittedRequestsService.apiSaveDraftRequestsPost(model).subscribe((id: string) => {
        this.requestsStore.reloadSubmittedRequests(this.data.request?.owner);
        this.onNoClick();
      })
    }   
  }

  public update() {
    if(this.formToSubmit && this.data.draft === true){
      const model: SubmittedRequestUpdateModel = {
        id: this.formToSubmit.id,
        sections: this.formToSubmit.sections
      }
      this.submittedRequestsService.apiSubmittedRequestsUpdate(model).subscribe((id: string) => {
        this.onNoClick();
      })
    }   
  }

  public multiIsChecked(question: SubmittedRequestQuestion, option: string): boolean {
    return question.answers.includes(option);
  }
  public multiSelect(question: SubmittedRequestQuestion, option: string) {
    if(!question.answers.includes(option)) {
      question.answers.push(option)
    } else {
      question.answers.splice(question.answers.indexOf(option), 1)
    }
    if(question.conditionalVisibilityTriggerForOtherQuestion.length > 0) {
      this.onAnswerChange(question);
    }
  }

  public isSectionRequiredDone(section: SubmittedRequestSection): boolean {
    let requiredDone = true;
    section.questions.forEach((q: SubmittedRequestQuestion) => {
      if(q.required === true && q.visible === true) {
        if(q.type === 'SingleChoice' || q.type === 'Dropdown' || q.type === 'Date' || q.type === 'AD' || q.type === 'Text') {
          if(q.answer.length === 0 || !q.answer) {
            requiredDone = false;
          } 
        } else if(q.type === 'MultipleChoice') {
          if(q.answers.length === 0 || !q.answers) {
            requiredDone = false;
          }
        } else if(q.type === 'Number') {
          if(q.answer === null || q.answer === undefined || q.answer === '') {
            requiredDone = false;
          }
        }
      }
    })
    return requiredDone;
  }

  onNoClick(): void {
    this.dialogRef.close('CLOSE');
  }
}
