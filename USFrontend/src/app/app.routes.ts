import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AboutComponent } from './about/about.component';
import { ShortUrlsComponent } from './short-urls/short-urls.component';
import { FullUrlComponent } from './full-url/full-url.component';

export const routes: Routes = [
    { path: '', component: ShortUrlsComponent },
    { path: 'login', component: LoginComponent },
    { path: 'about', component: AboutComponent },
    { path: 'full-info/:id', component: FullUrlComponent },
  ];
