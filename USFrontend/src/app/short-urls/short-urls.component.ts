import { Component } from '@angular/core';
import { ShortUrlDto } from '../models/shortUrlDto';
import { ShortUrlService } from '../services/shortUrlService';
import { NgFor, NgIf } from '@angular/common';
import { AuthService } from '../services/authService';
import { ShortUrlFullDto } from '../models/shortUrlFullDto';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms'; 

@Component({
  selector: 'app-short-urls',
  standalone: true,
  imports: [NgFor, NgIf, FormsModule],
  templateUrl: './short-urls.component.html',
  styleUrl: './short-urls.component.css'
})
export class ShortUrlsComponent {
  shortUrls: ShortUrlDto[] = [];
  newUrl: string = "";
  errorMessage: string = '';

  constructor(private shortUrlService: ShortUrlService, public authService: AuthService, private router: Router ) { }

  ngOnInit(): void {
    this.shortUrlService.getAllShortUrls().subscribe(
      (data: ShortUrlDto[]) => {
        console.log(data)
        this.shortUrls = data;
      },
      (error) => {
        console.error('Error fetching short URLs', error);
      }
    );
  }

  showInfo(id: string) {
    this.shortUrlService.getShortUrlById(id, this.authService.token).subscribe(
      (data: ShortUrlFullDto) => {
        this.router.navigate(['/full-info', id], { state: { urlInfo: data } });
      },
      (error) => {
        console.error('Error fetching short URL', error);
      }
    );
  }

  addUrl(){
    if (this.newUrl.trim()) {
      this.shortUrlService.addUrl(this.newUrl, this.authService.token).subscribe(
        (data) => {
          console.log('URL added successfully', data);
          this.newUrl = '';
          this.errorMessage = '';
          this.shortUrlService.getAllShortUrls().subscribe(
            (data: ShortUrlDto[]) => {
              console.log(data)
              this.shortUrls = data;
            },
            (error) => {
              console.error('Error fetching short URLs', error);
            }
          );
        },
        (error) => {
          if (error === "This URL already exists.") {
            this.errorMessage = "This URL already exists. Please try a different one.";
          } else {
            this.errorMessage = "Error adding URL. Please try again.";
          }
          console.error('Error adding URL', error);
        }
      );
    } else {
      this.errorMessage = "Please provide a valid URL.";
    }
  }

  deleteUrl(urlId: string) {
    this.shortUrlService.deleteUrl(urlId, this.authService.token).subscribe(
      () => {
        this.shortUrls = this.shortUrls.filter(url => url.id !== urlId);
      },
      (error) => {
        if (error.status === 403) {
          this.errorMessage = "You are not authorized to delete this URL.";
        } else {
          this.errorMessage = "An error occurred while deleting the URL.";
        }
      }
    );
  }

}
