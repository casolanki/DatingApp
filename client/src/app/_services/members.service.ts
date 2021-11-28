import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Member } from 'src/app/_models/member';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { noUndefined } from '@angular/compiler/src/util';

@Injectable({
  providedIn: 'root'
})

export class MembersService {

  baseUrl = environment.apiUrl;
  members: Member[];

  constructor(private http: HttpClient) { }

  getMembers() {
    if (this.members?.length > 0) return of(this.members);
    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(member => {
        this.members = member;
        return this.members;
      })
    )
  }

  getMember(username: string) {
    const member = (this.members?.length > 0) ? this.members.find(x => x.username = username)
        : undefined;
    if (member !== undefined) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    )
  }

  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }
}
