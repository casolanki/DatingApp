import { logging } from 'protractor';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from './../_services/account.service';
import { Observable } from 'rxjs';
import { User } from '../_model/user';
@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {} 

  constructor(public accountService: AccountService) {
  }

  ngOnInit() {   
  }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      console.log(response);
    }, error => {
      console.log(error);
    })
  }

  logout() {
    this.accountService.logout();
  }

}
