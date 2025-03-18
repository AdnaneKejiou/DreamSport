import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { selectTenantId } from '../../store/tenant/tenant.selectors';
import { Store } from '@ngrx/store';

export interface Terrain {
  id: number;
  title: string;
  description: string;
  image: string;
  terrainStatusId: number;
  idAdmin: number;
  idSport_Categorie: number;
  sport_Categorie: any;
  terrainStatus: any;
}

export interface TerrainStatus {
  id: number;
  libelle: string;
}

export interface SportCategory {
  id: number;
  name: string;
  nombreMax: number;
}

@Injectable({
  providedIn: 'root',
})
export class TerrainService {
  private apiUrl = 'http://localhost:5010/gateway/terrain';
  private tenantId: number | null = null;

  constructor(private http: HttpClient, private store: Store) {
    this.store.select(selectTenantId).subscribe((tenantId: number | null) => {
      this.tenantId = 28;
    });
  }

  getTerrains(): Observable<Terrain[]> {
    if (!this.tenantId) {
      throw new Error('Tenant ID non défini');
    }

    const headers = new HttpHeaders({
      'tenant-Id': this.tenantId.toString(),
    });

    return this.http.get<Terrain[]>(`${this.apiUrl}`, { headers });
  }

  getTerrainStatuses(): Observable<TerrainStatus[]> {
    if (!this.tenantId) {
      throw new Error('Tenant ID non défini');
    }

    const headers = new HttpHeaders({
      'tenant-Id': this.tenantId.toString(),
    });

    return this.http.get<TerrainStatus[]>('http://localhost:5010/gateway/terrainstatus', { headers });
  }

  getSportCategories(): Observable<SportCategory[]> {
    if (!this.tenantId) {
      throw new Error('Tenant ID non défini');
    }

    const headers = new HttpHeaders({
      'tenant-Id': this.tenantId.toString(),
    });

    return this.http.get<SportCategory[]>('http://localhost:5010/gateway/SportCategorie', { headers });
  }
}