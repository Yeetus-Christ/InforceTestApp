import { Component, NgModule } from '@angular/core';
import { AuthService } from '../services/authService';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Token } from '../models/token';

@Component({
  selector: 'app-login',
  imports: [NgIf, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  errorMessage: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    if (this.username && this.password) {
      this.authService.login(this.username, this.password).subscribe(
        (token: Token) => {
          this.authService.token = token.token;
          this,this.authService.loggedIn = true;
          console.log('Login successful, token:', token);
          this.router.navigate(['/']);
        },
        (error) => {
          this.errorMessage = 'Login failed. Please check your credentials.';
          console.error('Login error:', error);
        }
      );
    } else {
      this.errorMessage = 'Please enter both username and password.';
    }
  }
}