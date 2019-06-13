import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class UserService {
baseUrl = environment.baseUrl;

constructor(private http: HttpClient) { }
  getUsers(page?, itemsPerPage?, userParams?): Observable<PaginatedResult<User[]>>{
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();

    if(page != null && itemsPerPage !=null){
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if(userParams != null){
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }
    return this.http.get<User[]>(this.baseUrl + 'user', {observe: 'response', params}).
    pipe(
      map(response => {
        paginatedResult.result = response.body;
        if(response.headers.get('Pagination') != null){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'))
        }
        return paginatedResult;
      })
    );
  }

  getUser(id): Observable<User>{
    return this.http.get<User>(this.baseUrl + 'user/' + id);
  }

  updateUser(id: number, user: User){
    return this.http.put(this.baseUrl + 'user/' + id, user);
  }

  setMainPhoto(userId: number, id: number){    
    return this.http.post(this.baseUrl + 'user/' + userId + '/photos/' + id + '/setMain', {});
  }

  deletePhoto(userId: number, id: number){    
    debugger;
    return this.http.delete(this.baseUrl + 'user/' + userId + '/photos/' + id);
  }
}
