import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_service/auth.service';
import { AlertifyService } from '../_service/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next =>{
      this.alertify.success('Logged in successfully');
    }, error => {
      this.alertify.error('Failed to logged in.');
    });
  }

  loggedIn() {
    const token = localStorage.getItem('Item');
    return !!token;
  }

  loggedOut() {
    localStorage.removeItem('Item');
    this.alertify.message('Logged out successfully.');
  }

}
