import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { debounceTime, switchMap, distinctUntilChanged } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { SearchServiceService } from '../_service/SearchService.service';

@Component({
  selector: 'app-SearchFilterComponent',
  templateUrl: './SearchFilterComponent.component.html',
  styleUrls: ['./SearchFilterComponent.component.css']
})
export class SearchFilterComponentComponent implements OnInit {
  queryField: FormControl = new FormControl();
  baseUrl = 'http://localhost:5000/api/';
  results: any[];
  qeuryStringSecond: any[];
  searchedString: string;

  constructor(private http: HttpClient, private searchService: SearchServiceService) { }

  ngOnInit() {
    this.queryField.valueChanges
    .pipe(debounceTime(500))
    .pipe(switchMap(
      (query) => this.searchService.searchText(query)
    )).subscribe(
      res => this.results = res
      );
  }

  onClick(event: any){
    // debugger;
    // console.log(event.target.selectionStart);
    let space: number;
    let enter: number;
    this.searchedString = this.searchService.searchedString;
    space = this.searchedString.lastIndexOf(" ");
      enter = this.searchedString.lastIndexOf("\n");
      if(space < enter){
        let matchEnter = /\r|\n/.exec(this.searchedString);
        if(matchEnter){
          
          this.searchedString = this.searchedString.substring(0, this.searchedString.lastIndexOf("\n")+1);
        }else{
          this.searchedString = this.searchedString.substring(0, this.searchedString.lastIndexOf(" ")+1);
        }
      }else{
        this.searchedString = this.searchedString.substring(0, this.searchedString.lastIndexOf(" ")+1);
      }
    
 
    this.searchedString += event.target.textContent;
    this.queryField.patchValue(this.searchedString);

  }

  onClick1(event: any){
    debugger;
    this.searchedString = this.searchService.searchedString;
    let matchEnter = /\r|\n/.exec(this.searchedString);
    if(matchEnter){
      this.searchedString = this.searchedString.substring(0, this.searchedString.lastIndexOf("\n")+1);
    }else{
      this.searchedString = this.searchedString.substring(0, this.searchedString.lastIndexOf(" ")+1);
    }
 
    this.searchedString += event.target.textContent;
    this.queryField.patchValue(this.searchedString);

  }

//  searchTextFromApi (str): Observable<any> {
//     return this.http.get<any[]>(this.baseUrl + 'values/' + str);
//   }
}
