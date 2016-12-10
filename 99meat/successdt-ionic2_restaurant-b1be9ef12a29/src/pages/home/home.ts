import {Component} from '@angular/core';
import {NavController} from 'ionic-angular';
import {MenuService} from '../../services/menu-service';
import {CategoryPage} from "../category/category";
import { ValuesService } from '../../services/ValuesService';

/*
 Generated class for the LoginPage page.

 See http://ionicframework.com/docs/v2/components/#navigation for more info on
 Ionic pages and navigation.
 */
@Component({
  selector: 'page-home',
  templateUrl: 'home.html'
})
export class HomePage {
  // slides for slider 

  // list of categories
  //public categories: any;
public abc:any;
  constructor(public nav: NavController, public menuService: MenuService,private valuesService:ValuesService) {
    // set data for categories
		let currentUser = 	localStorage.getItem('UserInfo');
   	
	this.abc=	this.valuesService.getAll()
            .subscribe(
                data => {
									
		if(currentUser!=null)
		{
				localStorage.removeItem('UserInfo');
		}
                localStorage.setItem('UserInfo',JSON.stringify(data)); 
                }, 
                error => {
                  
                });
  }

  // view a category
  //viewCategory(categoryId) {
  //  this.nav.push(CategoryPage, {id: categoryId});
 // }
}
