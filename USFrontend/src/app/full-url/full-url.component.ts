import { Location, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { ShortUrlFullDto } from '../models/shortUrlFullDto';

interface NavigationState {
  urlInfo: ShortUrlFullDto;
}

@Component({
  selector: 'app-full-url',
  imports: [NgIf],
  templateUrl: './full-url.component.html',
  styleUrl: './full-url.component.css'
})
export class FullUrlComponent {
  urlInfo: ShortUrlFullDto | undefined;

  constructor(private location: Location) {}

  ngOnInit(): void {
    // Retrieve the URL info passed as state
    const navigation = this.location.getState() as NavigationState;
    if (navigation && navigation['urlInfo']) {
      this.urlInfo = navigation['urlInfo'];
    } else {
      console.error('No URL info found in state');
    }
  }
}
