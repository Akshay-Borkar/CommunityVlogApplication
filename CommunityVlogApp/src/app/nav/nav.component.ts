import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_service/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login() {
    this.authService.login(this.model).subscribe(next =>{
      console.log('Logged in successfully');
    }, error => {
      console.log('Failed to logged in.');
    });
  }

  loggedIn() {
    const token = localStorage.getItem('Item');
    return !!token;
  }

  loggedOut() {
    localStorage.removeItem('Item');
    console.log('Logged out successfully.');
  }

}
