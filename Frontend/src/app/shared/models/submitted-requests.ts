export interface SubmittedRequest {
  id: string;
  formName: string;
  formId: string;
  approverUID: string;
  approverName: string;
  owner: string;
  ownerName: string;
  icon: string;
  color: string;
  state: 'Approved' | 'Pending' | 'Rejected' | 'Draft';
  created: string;
  hideSections: boolean;
  description: string;
  sections: SubmittedRequestSection[]
}

export interface SubmittedRequestSection {
  sectionId: string;
  name: string;
  questions: SubmittedRequestQuestion[];
  requiredDone?: boolean;
}
  
export interface SubmittedRequestQuestion {
  questionId: string;
  text: string;
  required: boolean;
  type: 'SingleChoice' | 'MultipleChoice' | 'Text' | 'YesNo' | 'Date' | 'Dropdown' | 'AD' | 'Number';
  answer: string;
  answers: string[];
  minAnswer: number;
  maxAnswer: number;
  options: string[];
  conditionalVisibilityTriggerForOtherQuestion: SubmittedConditionalVisibility[];
  visible?: boolean; // Only for deciding visiblity in the submission form 
  booleanAnswer?: boolean; // Only for triggering boolean YesNo
}

export interface SubmittedConditionalVisibility {
  sectionId: string;
  sectionName: string;
  questionId: string;
  questionText: string;
  type: 'Equals' | 'NotEquals' | 'Contains' | 'NotContains';
  option: string;
  numberOption: number;
  options: string[];
  numberOptions: number[];
}

export interface SubmittedRequestSaveDraft {
  formId: string;
  formOwnerId: string;
  sections: SubmittedRequestSection[];
}

export interface SubmittedRequestUpdateModel {
  id: string;
  sections: SubmittedRequestSection[];
}