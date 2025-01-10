import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Token } from '../models/token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  private _token: string = "";
  private _loggedIn: boolean = false;
  private apiUrl: string = 'https://localhost:7155/api';

  get token(): string {
    return this._token;
  }

  set token(value: string) {
    this._token = value;
    this._loggedIn = value != null;
  }

  get loggedIn(): boolean {
    return this._loggedIn;
  }

  set loggedIn(value: boolean) {
    this._loggedIn = value;
    if (!value) {
      this._token = "";
    }
  }

  login(username: string, password: string): Observable<Token>{
    const loginPayload = { username, password };
    return this.http.post<Token>(`${this.apiUrl}/Auth`, loginPayload);
  }
}