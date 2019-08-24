import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-Signup',
  templateUrl: './Signup.component.html',
  styleUrls: ['./Signup.component.css']
})
export class SignupComponent implements OnInit {

  test: Date = new Date();
    focus;
    focus1;
  constructor() { }

  ngOnInit() {
  }

}
