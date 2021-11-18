import { Member } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[]
  constructor(private member: MembersService) { }

  ngOnInit() {
    this.loadMember()
  }

  loadMember() {
    this.member.getMembers().subscribe(resp => {
      this.members = resp
    });
  }

}
