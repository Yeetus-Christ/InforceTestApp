import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ShortUrlDto } from '../models/shortUrlDto';
import { ShortUrlFullDto } from '../models/shortUrlFullDto';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShortUrlService {

  private apiUrl: string = 'https://localhost:7155/api';

  constructor(private http: HttpClient) { }

  getAllShortUrls(): Observable<ShortUrlDto[]> {
    return this.http.get<ShortUrlDto[]>(`${this.apiUrl}/ShortUrl`);
  }

  getShortUrlById(id: string, token: string): Observable<ShortUrlFullDto>{
    const headers = new HttpHeaders().append('Authorization', 'Bearer'+' '+token);

    return this.http.get<ShortUrlFullDto>(`${this.apiUrl}/ShortUrl/${id}`, { headers });
  }

  addUrl(url: string, token: string): Observable<ShortUrlDto> {
    const headers = new HttpHeaders()
    .append('Authorization', 'Bearer ' + token)
    .append('Content-Type', 'application/json');

    return this.http.post<ShortUrlDto>(`${this.apiUrl}/ShortUrl`, JSON.stringify(url), { headers })
      .pipe(
        catchError(error => {
          if (error.status === 400) {
            return throwError("This URL already exists.");
          }
          return throwError(error);
        })
      );
  }

  deleteUrl(urlId: string, token: string): Observable<void> {
    const headers = new HttpHeaders().append('Authorization', 'Bearer ' + token);
    return this.http.delete<void>(`${this.apiUrl}/ShortUrl/${urlId}`, { headers });
  }
}