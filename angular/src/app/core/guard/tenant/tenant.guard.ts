import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import * as TenantActions from '../../store/tenant/tenant.actions';

@Injectable({
  providedIn: 'root'
})
export class TenantGuard implements CanActivate {
  constructor(
    private store: Store,
    private router: Router,
    private http: HttpClient
  ) {}

  canActivate(): Observable<boolean> {
    const tenantId = this.getTenantIdFromUrl(); // 🔥 Récupérer tenant depuis l'URL

    if (!tenantId) {
      this.router.navigate(['error/error404']);
      return of(false);
    }

    this.changeTenant(tenantId); // Met à jour le tenant

    // Vérification du tenant dans le backend
    return this.checkTenantExistence(tenantId).pipe(
      tap((exists) => {
        if (!exists) {
          this.router.navigate(['error/error404']); // Redirection vers la page d'erreur
        }
      }),
      map((exists) => exists) // Retourner true si le tenant existe, sinon false
    );
  }

  private getTenantIdFromUrl(): number | null {
    const urlParts = window.location.pathname.split('/');
    const tenantSlug = urlParts[1]; // Par exemple, '/club1/home' => 'club1'

    const tenantMap: { [key: string]: number } = {
      'club1': 1,
      'club2': 2,
      'club3': 3
    };

    if (window.location.hostname === 'localhost') {

      
      return 28;
    }

    return tenantMap[tenantSlug] || null; // Retourne le tenantId si trouvé, sinon null
  }

  private changeTenant(tenantId: number) {
    this.store.dispatch(TenantActions.loadTenantData({ tenantId }));
  
    // Abonnement au store pour surveiller les changements
    this.store.subscribe((state: any) => {
      if (state.tenant && state.tenant.siteInfo && state.tenant.siteInfo[0]) {
        const site = state.tenant.siteInfo[0];
  
        // Vérifier et appliquer les couleurs si présentes
        if (site.couleurPrincipale && site.couleurSecondaire) {
          this.applyMainColor(site.couleurPrincipale, site.couleurSecondaire);
        }
      }
    });
  }
  
  
  
  private applyMainColor(color: string , color2 :string) {
    document.documentElement.style.setProperty('--main-color', color);
    document.documentElement.style.setProperty('--sec-color', color2);
    const lightColor =this.primarylight(color);
    document.documentElement.style.setProperty('--main-light-color',lightColor );
    const successtColor =this.sucessColor(lightColor);
    document.documentElement.style.setProperty('--success-color',successtColor );

  }

  private primarylight(baseColor: string): string {
    const diff = { r: 26, g: 53, b: 24 }; // La différence calculée entre #097E52 et #23B33A
    
    // Convertir la couleur de base en RGB
    const r = parseInt(baseColor.slice(1, 3), 16);
    const g = parseInt(baseColor.slice(3, 5), 16);
    const b = parseInt(baseColor.slice(5, 7), 16);
    
    // Appliquer la différence à chaque composant et s'assurer que la nouvelle couleur est dans les limites (0-255)
    const newR = Math.min(255, Math.max(0, r + diff.r));
    const newG = Math.min(255, Math.max(0, g + diff.g));
    const newB = Math.min(255, Math.max(0, b + diff.b));
    
    // Convertir de nouveau en format hexadécimal
    return `#${newR.toString(16).padStart(2, '0')}${newG.toString(16).padStart(2, '0')}${newB.toString(16).padStart(2, '0')}`;
}
 private sucessColor(baseColor: string): string {
  const diff = { r: 2, g: 6, b: 3 }; // La différence calculée entre #23B33A et #1BB333
  
  // Convertir la couleur de base en RGB
  const r = parseInt(baseColor.slice(1, 3), 16);
  const g = parseInt(baseColor.slice(3, 5), 16);
  const b = parseInt(baseColor.slice(5, 7), 16);
  
  // Appliquer la différence à chaque composant et s'assurer que la nouvelle couleur est dans les limites (0-255)
  const newR = Math.min(255, Math.max(0, r + diff.r));
  const newG = Math.min(255, Math.max(0, g + diff.g));
  const newB = Math.min(255, Math.max(0, b + diff.b));
  
  // Convertir de nouveau en format hexadécimal
  return `#${newR.toString(16).padStart(2, '0')}${newG.toString(16).padStart(2, '0')}${newB.toString(16).padStart(2, '0')}`;
}

  private checkTenantExistence(tenantId: number): Observable<boolean> {
    const headers = new HttpHeaders().set('Tenant-ID', tenantId.toString());
  
    // Effectuer une requête HTTP pour vérifier l'existence du tenant
    const url = `http://localhost:5010/gateway/Admin/validate`;
    return this.http.get<any>(url, { headers }).pipe(
      catchError((error) => {
        if (error.status === 404) {
          return of(false);
        }
        return of(false); // Retourner false en cas d'erreur serveur ou réseau
      }),
      map((response) => {
        return response ? true : false; // Retourner true si AdminId est retourné
      })
    );
  }
  
  
}
