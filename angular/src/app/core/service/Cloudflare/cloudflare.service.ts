import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CloudflareService {
  // L'URL de ton bucket Cloudflare R2
  private bucketUrl = 'https://e3e3b386259734acfcc14178a94ae982.r2.cloudflarestorage.com'; // Remplace par ton URL R2
  private accessKey = '6bf5ee7ffccf0841651dee0e9b6be0ca'; // Remplace par ta clé d'accès
  private secretKey = 'a8ffa9a85b1d693620de19424e35edd692642e1e0c8c18e170ffb100d841e30b'; // Remplace par ta clé secrète

  constructor(private http: HttpClient) {}

  /**
   * Upload une image sur Cloudflare R2 et retourne son URL.
   * @param file Fichier image à uploader
   */
  uploadImage(file: File): Observable<string> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.secretKey}`
    });

    return new Observable(observer => {
      this.http.put(`${this.bucketUrl}/${file.name}`, file, { headers }).subscribe(
        () => {
          observer.next(`${this.bucketUrl}/${file.name}`);
          observer.complete();
        },
        error => observer.error(error)
      );
    });
  }

  /**
   * Récupère une image à partir de son URL et retourne un Blob.
   * @param imageUrl URL de l'image stockée sur Cloudflare
   */
  getImage(imageUrl: string): Observable<Blob> {
    return this.http.get(imageUrl, { responseType: 'blob' });
  }
}
