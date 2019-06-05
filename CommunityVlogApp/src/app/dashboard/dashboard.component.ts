import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  values: any;
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  getValues() {
    // this.http.get('http://localhost:5000/api/values').subscribe(response => {
    //   this.values = JSON.parse(JSON.stringify(response));
    //   console.log(response);
    // }, error => {
    //   console.log(error);
    // });

  }

}
