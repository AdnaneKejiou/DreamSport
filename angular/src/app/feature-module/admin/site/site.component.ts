import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SiteService } from 'src/app/core/service/Backend/SIte/site.service';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { SiteM } from 'src/app/core/models/Site/siteM';
import { CloudflareService } from 'src/app/core/service/Cloudflare/cloudflare.service';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-site',
  templateUrl: './site.component.html',
  styleUrls: ['./site.component.scss']
})
export class SiteComponent implements OnInit {
  siteForm: FormGroup;
  isSaving = false;
  isUploading = false;
  currentUploadField: string | null = null;
  private Site: any;

  constructor(
    private fb: FormBuilder,
    private siteService: SiteService,
    private toastr: ToastrService,
    private cloudflareService: CloudflareService,
  ) {
    this.siteForm = this.fb.group({
      Id: [''],
      Name: ['', Validators.required],
      Logo: [''],
      Description: [''],
      Email: ['', [Validators.email]],
      PhoneNumber: [''],
      AboutUs: [''],
      CouleurPrincipale: ['#3f51b5', [Validators.pattern(/^#([0-9A-F]{3}){1,2}$/i)]],
      CouleurSecondaire: ['#ff4081', [Validators.pattern(/^#([0-9A-F]{3}){1,2}$/i)]],
      Background: [''],
      Addresse: [''],
      DomainName: ['', [Validators.pattern(/^[a-zA-Z0-9][a-zA-Z0-9-]{1,61}[a-zA-Z0-9](?:\.[a-zA-Z]{2,})+$/)]]
    });
  }

  ngOnInit(): void {
    this.loadSiteSettings();
  }

  loadSiteSettings(): void {
    this.siteService.getSiteSettings().subscribe({
      next: (settings) => {
        const siteSettings = Array.isArray(settings) ? settings[0] : settings;
        this.Site = siteSettings;

        this.siteForm.patchValue({
          Id: siteSettings.id,
          Name: siteSettings.name,
          Logo: siteSettings.logo,
          Description: siteSettings.description,
          Email: siteSettings.email,
          PhoneNumber: siteSettings.phoneNumber,
          AboutUs: siteSettings.aboutUs,
          CouleurPrincipale: siteSettings.couleurPrincipale,
          CouleurSecondaire: siteSettings.couleurSecondaire,
          Background: siteSettings.background,
          Addresse: siteSettings.addresse,
          DomainName: siteSettings.domainName
        });
      },
      error: (err: HttpErrorResponse) => {
        console.error("Error loading settings:", err);
        this.toastr.error('Failed to load site settings', 'Error');
      }
    });
  }

  async onFileSelected(event: Event, field: string): Promise<void> {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    if (!file.type.match('image.*')) {
      this.toastr.warning('Please select an image file', 'Invalid File');
      return;
    }

    this.isUploading = true;
    this.currentUploadField = field;

    try {
      // Upload to Cloudflare CDN
      const imageUrl = await this.cloudflareService.uploadImage(file).toPromise();
      
      // Update form with CDN URL
      this.siteForm.get(field)?.setValue(imageUrl);
      this.toastr.success('Image uploaded successfully', 'Success');
    } catch (error) {
      console.error('Upload failed:', error);
      this.toastr.error('Failed to upload image', 'Error');
    } finally {
      this.isUploading = false;
      this.currentUploadField = null;
      input.value = ''; // Reset input to allow same file re-upload
    }
  }

  removeBackground(): void {
    this.siteForm.get('Background')?.setValue('');
  }

  resetForm(): void {
    this.siteForm.reset();
    this.loadSiteSettings();
  }

  saveSiteSettings(): void {
    if (this.siteForm.invalid) {
      this.siteForm.markAllAsTouched();
      this.toastr.warning('Please fill all required fields correctly', 'Form Invalid');
      return;
    }

    this.isSaving = true;
    this.siteService.updateSiteSettings(this.siteForm.value)
      .pipe(finalize(() => this.isSaving = false))
      .subscribe({
        next: () => this.toastr.success('Site settings saved successfully!', 'Success'),
        error: (err: HttpErrorResponse) => {
          console.error('Save error:', err);
          this.toastr.error('Failed to save site settings', 'Error');
        }
      });
  }

  onColorChange(field: string, event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input) {
      this.siteForm.get(field)?.setValue(input.value);
    }
  }

  onTextColorChange(field: string, event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = input.value;
    
    if (/^#([0-9A-F]{3}){1,2}$/i.test(value)) {
      this.siteForm.get(field)?.setValue(value);
    } else {
      this.toastr.warning('Please enter a valid hex color code', 'Invalid Color');
    }
  }
}