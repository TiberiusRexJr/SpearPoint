import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private base = environment.apiBase;

  getHealth() { return this.http.get('/healthz', { responseType: 'text' }); }
  getQuestions() { return this.http.get<{ id:number; text:string; choices:string[] }[]>(`${this.base}/questions`); }
}
