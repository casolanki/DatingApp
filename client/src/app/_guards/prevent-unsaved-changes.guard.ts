import { MemberEditComponent } from './../members/member-edit/member-edit.component';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

//CanDeactivate helps to promt confirmation message, when you try to exit
//perticular component without any save changes
export class PreventUnsavedChangesGuard implements CanDeactivate<unknown> {
  canDeactivate(
    component: MemberEditComponent): boolean {
      if(component.editForm.dirty){
        return confirm('Are you sure you want to continue? Any unsaved changes will be lost')
      }
    return true;
  }
  
}
