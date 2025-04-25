import { Component, ElementRef, HostListener } from '@angular/core';
import { Router } from '@angular/router';
import { MsalBroadcastService, MsalService } from '@azure/msal-angular';
import { Contact, MonitorCog, BookText, ChartBar, Bell, CircleCheck, LogOut } from 'lucide-angular';
import { NotificationsService } from '../../services/entities/notifications.service';
import { UserStore } from '../../services/stores/user.store';
import { SubmittedRequestsService } from '../../services/entities/submitted-requests.service';
import { Notification } from '../../shared/models/notification';
import { SubmittedRequest } from '../../shared/models/submitted-requests';
import { MatDialog } from '@angular/material/dialog';
import { ApproveRequestComponent } from '../requests/approve-request/approve-request.component';
import { RequestsService } from '../../services/entities/requests.service';
import { FormQuestion, FormRequest, FormSection } from '../../shared/models/form-request';
import jsPDF from 'jspdf';
import { SubmitRequestComponent } from '../requests/submit-request/submit-request.component';

export interface LoadingState {
  pinnedRequests: boolean;
  pinnedManagedRequests: boolean;
  formRequests: boolean;
  manageRequests: boolean;
  myRequests: boolean;
  requestsToApprove: boolean;
}

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  constructor(
    private router: Router,
    public userStore: UserStore,
    private notifcationService: NotificationsService,
    private formRequestsService: RequestsService,
    private submittedRequestsService: SubmittedRequestsService,
    private dialog: MatDialog,
    private elementRef: ElementRef) { 
      fetch('../../../../assets/fonts/Inter.txt')
      .then(response => response.text())
      .then(fontBase64 => {
        this.base64RobotoFont = fontBase64;
      })
      .catch(error => {
        console.error('Failed to load Roboto font:', error);
      });
    }

  ngOnInit(): void {
    if(this.userStore.currentAccount) {
      this.notifcationService.apiNotificationGet().subscribe((data => {
        this.userStore.currentNotifications = data;
      }))
      this.formRequestsService.apiFormRequestsGet().subscribe((data: FormRequest[]) => {
        this.pinnedRequests = data.slice(0, 3);
        this.allRequest = data;
        this.loadingState.pinnedRequests = false;
        this.loadingState.formRequests = false;
      })
      this.formRequestsService.apiFormRequestsGetOwner().subscribe((data: FormRequest[]) => {
        this.pinnedManagedRequests = data.slice(0, 3);
        this.allManagedRequests = data;
        this.loadingState.pinnedManagedRequests = false;
        this.loadingState.manageRequests = false;
      })
      this.submittedRequestsService.apiApprovalRequestsGet(this.userStore.currentAccount.localAccountId).subscribe((data: SubmittedRequest[]) => {
        this.requestsToApprove = data;
        this.loadingState.requestsToApprove = false;
      })
      this.submittedRequestsService.apiSubmittedRequestsGet(this.userStore.currentAccount.localAccountId).subscribe((data: SubmittedRequest[]) => {
        this.submittedRequests = data;
        this.loadingState.myRequests = false;
      })
    }
  }

  public loadingState: LoadingState = {
    pinnedRequests: true,
    pinnedManagedRequests: true,
    formRequests: true,
    manageRequests: true,
    myRequests: true,
    requestsToApprove: true
  }
  public manageMode = false;
  public switchManageMode() {
    this.manageMode = !this.manageMode;
  }

  public pinnedRequests: FormRequest[] = [];
  public pinnedManagedRequests: FormRequest[] = [];
  public allRequest: FormRequest[] = [];
  public allManagedRequests: FormRequest[] = [];
  public submittedRequests: SubmittedRequest[] = [];
  public requestsToApprove: SubmittedRequest[] = [];

  public statuses = ['Approved', 'Rejected', 'Submitted'];

  active: 'main-1' | 'main-2' = 'main-1';

  switchTo(next: 'main-1' | 'main-2') {
    this.active = next;
  }

  public readonly LogOut = LogOut;
  public readonly Contact = Contact;
  public readonly Bell = Bell;
  public readonly CircleCheck = CircleCheck;

  public isNotificationScrenOpen = false;

  openNotificationScreen() {
    this.isNotificationScrenOpen = true;
  }
  closeNotifications() {
    this.isNotificationScrenOpen = false;
  }

  // navigateTo(path: string) {
  //   this.router.navigate(['/' + path]);
  // }

  // isActive(path: string): boolean {
  //   return this.router.url.includes(path);
  // }

  navigateToApproveRequestFromNotif(notification: Notification, nIndex: number) {
    this.submittedRequestsService.apiSubmtitedRequestGet(notification.submittedRequestId, notification.requesterUID).subscribe((data: SubmittedRequest | null) => {
      if(data) {
        const dialogRef = this.dialog.open(ApproveRequestComponent, {
          panelClass: 'dialog-box',
          data: { viewOnly: false, submittedRequest: data }
        })
        this.notifcationService.apiNotificationDelete(notification.id).subscribe(() => {
          this.userStore.currentNotifications.splice(nIndex, 1);
        })
        dialogRef.afterClosed().subscribe((data: string) => {
          if(data === "DONE") {
            //this.initializeDataApprove(); //TODO
          }
        })
      }
    })
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    if(this.isNotificationScrenOpen === true){
      const notificationsElement = this.elementRef.nativeElement.querySelector('#notifications');
      if (this.isNotificationScrenOpen && notificationsElement && !notificationsElement.contains(event.target)) {
        this.closeNotifications();
      }
    }  
  }

  // PDF Generator
  private base64RobotoFont!: string;
  public isDownloadInProgress = false;
  
  public async onDownloadPDF(request: FormRequest) {

    const backgroundColor = '#FFFFFF';

    // ICONS
    const radioButton = await this.loadSvgAsBase64('assets/icons/radio-button.svg');
    const checkbox = await this.loadSvgAsBase64('assets/icons/checkbox.svg');


    if(this.isDownloadInProgress === false){

      this.isDownloadInProgress = true;
  
      const pdf = new jsPDF('p', 'mm', 'a4');

      let currentY = 10; // Initial Y position
      const margin = 12;
      const pageHeight = pdf.internal.pageSize.getHeight();

      pdf.setFillColor(backgroundColor);
      pdf.rect(0, 0, pdf.internal.pageSize.getWidth(), pdf.internal.pageSize.getHeight(), 'F');

      pdf.addFileToVFS("Roboto.ttf", this.base64RobotoFont);
      pdf.addFont("Roboto.ttf", "Roboto", "normal");
      pdf.setFont('Roboto');

      pdf.setTextColor('#000000');

      const checkForNextPage = (height: number) => {
        if (currentY + height + margin > pageHeight) {
          pdf.addPage();
          currentY = margin;
          pdf.setFillColor(backgroundColor);
          pdf.rect(0, 0, pdf.internal.pageSize.getWidth(), pdf.internal.pageSize.getHeight(), 'F');
        } 
      }

      const writeText = (text: string, fontSize: number, marginDown: number) => {
        pdf.setFontSize(fontSize)
        const textLines = pdf.splitTextToSize(text, pdf.internal.pageSize.getWidth() - 2 * margin);
        const textHeight = textLines.length * 5;
        checkForNextPage(textHeight)
        pdf.text(textLines, margin, currentY);
        currentY += textHeight + marginDown; 
      }

      const writeScale = ( options: string[], iconType: string, statementText?: string) => {
        if(statementText && statementText !== "") {
          writeText(statementText, 8, 0)
        }

        const iconSize = 4;
        const pageWidth = pdf.internal.pageSize.getWidth(); // Width of the page
        const lineHeight = 6; // Vertical space between lines
        const spacing = 6; // Horizontal space between options
        const maxWidth = pageWidth - margin * 2; // Usable width of the page
        let currentX = margin; // Initial X position

        options.forEach((option) => {
          const textWidth = pdf.getTextWidth(option);
          const itemWidth = Math.max(textWidth, iconSize); // Max width of icon or text
      
          // Check if the current option fits on the row
          if (currentX + itemWidth > maxWidth) {
            // Move to the next row
            currentX = margin;
            currentY += lineHeight + iconSize + 2;
          }
      
          // Draw the icon
          if(iconType === "radio" ) {
            pdf.addImage(radioButton, "PNG", currentX + (itemWidth / 2) - 2, currentY, iconSize, iconSize);
          } else if(iconType === "checkbox") {
            pdf.addImage(checkbox, "PNG", currentX + (itemWidth / 2) - 2, currentY, iconSize, iconSize);
          } 
          // Write the option at the current position
          pdf.text(option, currentX, currentY + iconSize + 4);
      
          // Update the X position for the next option
          currentX += itemWidth + spacing;
        });
        currentY += 16;
      }

      const writeInputField = (height: number = 8) => {
        const pageWidth = pdf.internal.pageSize.getWidth();
        const inputWidth = pageWidth - (margin * 2);
        
        // Set the border color to a light gray
        pdf.setDrawColor(200, 200, 200);
        pdf.setLineWidth(0.35);
        
        // Draw rounded rectangle
        const radius = 1;
        const x = margin;
        const y = currentY;
        
        // Draw the rounded rectangle
        pdf.roundedRect(x, y, inputWidth, height, radius, radius);
        
        // Add some padding inside the input field
        currentY += height + 10;
      }
 
      // if(this.userPreferences?.bannerURL) {
      //   let fillColor = "#ffffff"
      //   if(this.themeService.brandUserTheme) { 
      //     const brandColors = this.themeService.brandUserTheme.split(";")
      //     if(brandColors.length === 2) { 
      //       fillColor = brandColors[0]
      //     }
      //   }
        
      //   const image = await this.preprocessImageWithBackground(this.userPreferences?.bannerURL, fillColor);
      //   const x = 0; // X position on the page
      //   const y = 0; // Y position on the page
      //   const width = pdf.internal.pageSize.getWidth();
      //   const height = 30; // Desired height of the image

      //   pdf.addImage(image, 'PNG', x, y, width, height);

      //   currentY += 32;
      // }

      writeText(request.name, 16, 6)
      writeText(request.description, 10, 6)

      request.sections.forEach((section: FormSection) => {
        if(request.hideSections === false){
          writeText(section.name, 12, 4)
        }
        section.questions.forEach((question: FormQuestion) => {
          if(question.type === 'SingleChoice' || question.type === 'Dropdown') {
            writeText(question.text, 9, 0)
            writeScale(question.options, "radio")
          } else if(question.type === 'MultipleChoice') {
            writeText(question.text, 9, 0)
            writeScale(question.options, "checkbox")
          } else if(question.type === 'YesNo') {
            writeText(question.text, 9, 0)
            writeScale(["Yes", "No"], "radio")
          } else if(question.type === 'Text' || question.type === 'AD' || question.type === 'Date' ) {
            writeText(question.text, 9, 0)
            writeInputField()
          }
        })
      });

      // END
      pdf.save(request.name + '.pdf');
      this.isDownloadInProgress = false;
    }
  }

  private async preprocessImageWithBackground(imageUrl: string, backgroundColor: string): Promise<string> {
    return new Promise((resolve, reject) => {
      const img = new Image();
      img.crossOrigin = 'Anonymous'; // Enable CORS
      img.src = imageUrl;
  
      img.onload = () => {
        const canvas = document.createElement('canvas');
        canvas.width = img.width;
        canvas.height = img.height;
        const ctx = canvas.getContext('2d');
  
        if (ctx) {
          // Fill canvas with the background color
          ctx.fillStyle = backgroundColor; // e.g., 'white', '#f0f0f0', 'rgba(255, 255, 255, 1)'
          ctx.fillRect(0, 0, canvas.width, canvas.height);
  
          // Draw the PNG image on top of the filled background
          ctx.drawImage(img, 0, 0);
  
          // Convert canvas to Base64
          const base64Image = canvas.toDataURL('image/png');
          resolve(base64Image);
        } else {
          reject('Canvas context not available');
        }
      };
  
      img.onerror = (error) => reject(error);
    });
  }

  private loadSvgAsBase64(svgPath: string): Promise<string> {
    return fetch(svgPath)
      .then(response => response.text())
      .then(svgText => {
        const svgBlob = new Blob([svgText], { type: 'image/svg+xml' });
        const url = URL.createObjectURL(svgBlob);
        return new Promise(resolve => {
          const img = new Image();
          img.onload = () => {
            const canvas = document.createElement('canvas');
            canvas.width = img.width;
            canvas.height = img.height;
            const context = canvas.getContext('2d');
            context?.drawImage(img, 0, 0);
            URL.revokeObjectURL(url);
            resolve(canvas.toDataURL('image/png'));
          };
          img.src = url;
        });
      });
  }

  getStateClass(state: string): string {
    switch (state) {
      case 'Approved':
        return 'bg-green-100 text-green-800';
      case 'Submitted':
        return 'bg-yellow-100 text-yellow-800';
      case 'Rejected':
        return 'bg-red-100 text-red-800';
      default:
        return '';
    }
  }
  
  navigateToApproveRequest(submittedRequest: SubmittedRequest) {
    const dialogRef = this.dialog.open(ApproveRequestComponent, {
      panelClass: 'dialog-box',
      data: { viewOnly: false, submittedRequest: submittedRequest }
    })
    dialogRef.afterClosed().subscribe((data: string) => {
      if(data === "DONE") {
        //this.initializeDataApprove();
      }
    })
  }

  navigateToEditRequest(submittedRequest: SubmittedRequest) {
    const dialogRef = this.dialog.open(SubmitRequestComponent, {
      panelClass: 'dialog-box',
      data: { draft: true, request: null, submittedRequest: submittedRequest }
    })
    dialogRef.afterClosed().subscribe((data: string) => {
      if(data === 'SUBMIT') {
        //this.initializeDataMyRequests();
      }
    })
  }

  navigateToViewRequest(submittedRequest: SubmittedRequest) {
    const dialogRef = this.dialog.open(ApproveRequestComponent, {
      panelClass: 'dialog-box',
      data: { viewOnly: true, submittedRequest: submittedRequest }
    })
    dialogRef.afterClosed().subscribe((data: string) => {
      
    })
  }
}
