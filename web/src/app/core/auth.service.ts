import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private _accessToken = signal<string | null>(null);

  accessToken() { return this._accessToken(); }
  setToken(token: string | null) { this._accessToken.set(token); }

  // demo login: set a fake token
  demoLogin() { this.setToken('demo-token'); }
  logout() { this.setToken(null); }
}
