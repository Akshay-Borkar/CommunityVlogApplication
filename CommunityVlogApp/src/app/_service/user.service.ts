import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class UserService {
baseUrl = environment.baseUrl;

constructor(private http: HttpClient) { }
  getUsers(): Observable<User[]>{
    return this.http.get<User[]>(this.baseUrl + 'user');
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
}
