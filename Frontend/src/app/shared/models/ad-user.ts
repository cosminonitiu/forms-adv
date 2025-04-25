export interface AdditionalData {
  additionalProp1: string;
  additionalProp2: string;
  additionalProp3: string;
}

export interface BackingStore {
  initializationCompleted: boolean;
  returnOnlyChangedValues: boolean;
}

export interface AppRoleAssignment {
  additionalData: AdditionalData;
  backingStore: BackingStore;
  id: string;
  odataType: string;
  deletedDateTime: string;
  appRoleId: string;
  createdDateTime: string;
  principalDisplayName: string;
  principalId: string;
  principalType: string;
  resourceDisplayName: string;
  resourceId: string;
}

export interface AuthorizationInfo {
  additionalData: AdditionalData;
  backingStore: BackingStore;
  certificateUserIds: string[];
  odataType: string;
}

export interface DirectoryObject {
  additionalData: AdditionalData;
  backingStore: BackingStore;
  id: string;
  odataType: string;
  deletedDateTime: string;
}

export interface EmployeeOrgData {
  additionalData: AdditionalData;
  backingStore: BackingStore;
  costCenter: string;
  division: string;
  odataType: string;
}

export interface Identity {
  additionalData: AdditionalData;
  backingStore: BackingStore;
  issuer: string;
  issuerAssignedId: string;
  odataType: string;
  signInType: string;
}

export interface ADUser {
  id: string;
  appRoleAssignments: AppRoleAssignment[];
  authorizationInfo: AuthorizationInfo;
  country: string;
  department: string;
  directReports: DirectoryObject[];
  displayName: string;
  employeeId: string;
  employeeOrgData: EmployeeOrgData;
  identities: Identity[];
  jobTitle: string;
  mail: string;
  manager: DirectoryObject;
  memberOf: DirectoryObject[];
  mobilePhone: string;
  userPrincipalName: string;
}
