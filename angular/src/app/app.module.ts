import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { sharedModule } from './shared/shared.module';
import { DatePipe } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { tenantReducer } from './core/store/tenant/tenant.reducer';
import { TenantEffects } from './core/store/tenant/tenant.effects';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    sharedModule,
    StoreModule.forRoot({ tenant: tenantReducer }),
    EffectsModule.forRoot([TenantEffects])
  ],
  providers: [
    DatePipe,
    provideHttpClient(withInterceptorsFromDi()), 
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
