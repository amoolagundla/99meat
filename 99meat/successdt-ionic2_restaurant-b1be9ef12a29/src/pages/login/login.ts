import {Component} from '@angular/core';
import {NavController} from 'ionic-angular';
import {HomePage} from "../home/home";
import {AuthenticationService} from "../../services/login-service";
import {ValuesService} from "../../services/ValuesService";
import { LoadingController } from 'ionic-angular';
import { AlertController } from 'ionic-angular';

import {RegisterPage} from "../register/register";

/*
 Generated class for the LoginPage page.

 See http://ionicframework.com/docs/v2/components/#navigation for more info on
 Ionic pages and navigation.
 */
@Component({
  selector: 'page-login',
  templateUrl: 'login.html'
})
export class LoginPage {

public username:string;
public token:string;
public loading :any= this.loadingCtrl.create({
      content: "Please wait...",      
      dismissOnPageChange: true
    });
public password:string;
  constructor(public nav: NavController, 
	            private authenticationService: AuthenticationService,
							public loadingCtrl: LoadingController,
							public valuesService: ValuesService,
							public alertCtrl: AlertController) {
								localStorage.removeItem("UserInfo");
						localStorage.removeItem('currentUser');		
		this.token= JSON.parse(localStorage.getItem('currentUser'));
		if(this.token!=null)
		{
	          	this.loading.present();
		         this.valuesService.getAll().subscribe(
                data => {
									 this.loading.dismiss();
                    this.nav.setRoot(HomePage);
                   
                }, 
                error => {
																	this.loading.dismiss();
																	let alert = this.alertCtrl.create({
											title: 'Login Failed',
											subTitle: 'Please login again :)',
											buttons: ['OK']
										});
										alert.present();
                  // this.nav.setRoot(RegisterPage);
                });
		}
  }

   // go to register page
  register() {
    this.nav.setRoot(RegisterPage);
  }
  // login and go to home page
  login() { 
	
		this.loading.present();
		 this.authenticationService.login(this.username, this.password)
            .subscribe(
                data => {
									
                    this.nav.setRoot(HomePage);
  
                }, 
                error => {
									this.loading.dismiss();
                  // this.nav.setRoot(RegisterPage);
                });
    
  }
}

