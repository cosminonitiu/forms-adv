export interface FormRequest {
  id: string;
  name: string;
  owner: string;
  icon: string;
  created: string;
  description: string;
  color: string;
  hideSections: boolean;
  sections: FormSection[]
}

export interface FormSection {
  id: string;
  name: string;
  questions: FormQuestion[];
}
  
export interface FormQuestion {
  id: string;
  text: string;
  type: 'SingleChoice' | 'MultipleChoice' | 'Text' | 'YesNo' | 'Date' | 'Dropdown' | 'AD';
  options: string[];
  required: boolean;
  conditionalVisibilities: ConditionalVisibility[];
}

export interface FormQuestionNoConditionals { //Strictly for COnditional Visibiltiy Selection
  id: string;
  text: string;
  type: 'SingleChoice' | 'MultipleChoice' | 'Text' | 'YesNo' | 'Date' | 'Dropdown' | 'AD';
  options: string[];
}


export interface ConditionalVisibility {
  sectionId: string;
  sectionName: string;
  questionId: string;
  questionText: string;
  type: 'Equals' | 'NotEquals' | 'Contains' | 'NotContains';
  option: string;
  options: string[];
  possibleQuestions?: FormQuestionNoConditionals[]; //Strictly for COnditional Visibiltiy Selection
}