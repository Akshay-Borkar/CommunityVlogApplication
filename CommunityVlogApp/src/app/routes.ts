import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersComponent } from './member/members/members.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './member/member-detail/member-detail.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'members', component: MembersComponent },
            { path: 'members/:id', component: MemberDetailComponent },
            { path: 'messages', component: MessagesComponent },
            { path: 'list', component: ListsComponent },
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
