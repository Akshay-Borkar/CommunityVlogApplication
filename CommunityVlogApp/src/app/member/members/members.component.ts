import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { AlertifyService } from '../../_service/alertify.service';
import { UserService } from '../../_service/user.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.css']
})
export class MembersComponent implements OnInit {
  users: User[];
  pagination: Pagination;

  constructor(private userService: UserService, private alertify: AlertifyService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data =>
      {
        this.users = data['users'].result;
        this.pagination = data['users'].pagination;
      });
  }
  pageChanged(event: any): void{
    this.pagination.currentPage = event.page;
    this.getUsers();
  }
  

  getUsers(){ 
    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe((res: PaginatedResult<User[]>) => {
      this.users = res.result;
      this.pagination = res.pagination;
    }, error => {
      this.alertify.error(error);
    });
  }

}
