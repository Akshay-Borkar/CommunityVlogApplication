import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthService } from './_service/auth.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthService){}

  ngOnInit() {
    const token = localStorage.getItem('Item');
    const user: User = JSON.parse(localStorage.getItem('user'));
    if(token){
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }

    if(user){
      this.authService.currentUser = user;
      this.authService.changeMemberPhoto(user.photoUrl);
    }
  }



}
