import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeatureModuleComponent } from './feature-module.component';
import { TenantGuard } from 'src/app/core/guard/tenant/tenant.guard'; // Importer le TenantGuard

const routes: Routes = [
  {
    path: '',
    component: FeatureModuleComponent,
    children: [
      {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full',
      },
      {
        path: 'blog',
        loadChildren: () =>
          import('./blog/blog.module').then((m) => m.BlogModule),
        canActivate: [TenantGuard], 
      },
      {
        path: 'coaches',
        loadChildren: () =>
          import('./coaches/coaches.module').then((m) => m.CoachesModule),
        canActivate: [TenantGuard], 
      },
      {
        path: 'pages',
        loadChildren: () =>
          import('./pages/pages.module').then((m) => m.PagesModule),
        canActivate: [TenantGuard], 
      },
      {
        path: 'auth',
        loadChildren: () =>
          import('./auth/auth.module').then((m) => m.AuthModule),
        canActivate: [TenantGuard], 
      },
      {
        path: 'user',
        loadChildren: () =>
          import('./user/user.module').then((m) => m.UserModule),
        canActivate: [TenantGuard], 
      },
      {
        path: 'home',
        loadChildren: () =>
          import('./home/home.module').then((m) => m.HomeModule),
        canActivate: [TenantGuard], // Appliquer le Guard ici
      },
      {
        path: 'listings',
        loadChildren: () =>
          import('./listings/listings.module').then((m) => m.ListingsModule),
        canActivate: [TenantGuard], // Appliquer le Guard ici aussi
      },
      {
        path: 'admin',
        loadChildren: () =>
          import('./admin/admin.module').then((m) => m.AdminModule),
        canActivate: [TenantGuard], // Appliquer le Guard ici aussi
      },
    ],
  },
  {
    path: 'error',
    loadChildren: () =>
      import('./error/error.module').then((m) => m.ErrorModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class FeatureModuleRoutingModule {}
