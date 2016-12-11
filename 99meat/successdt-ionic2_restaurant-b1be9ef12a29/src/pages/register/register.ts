import {Component,OnInit} from '@angular/core';
import {NavController} from 'ionic-angular';
import {LoginPage} from "../login/login";
import { ValuesService } from '../../services/ValuesService';
import { ToastController } from 'ionic-angular';
import { LoadingController } from 'ionic-angular';
/*
 Generated class for the LoginPage page.

 See http://ionicframework.com/docs/v2/components/#navigation for more info on
 Ionic pages and navigation.
 */
@Component({
  selector: 'page-register',
  templateUrl: 'register.html'
})
export class RegisterPage implements OnInit {
 public user: any;
 public errorMessage:any;
 public loading :any= this.loadingCtrl.create({
      content: "Please wait...",      
      dismissOnPageChange: true
    });
 
 
  constructor(public nav: NavController,private valuesService:ValuesService,public toastCtrl: ToastController,
		public loadingCtrl: LoadingController) {
  }
 ngOnInit() {
    this.user = {
      email: '',
      password: '',
      confirmPassword: '',
			phonenumber:''
    }
  }

  save(model: any, isValid: boolean) {
    // call API to save customer
    
									
		if(isValid)
		{
			this.loading.present();
			this.valuesService.Register(model)
            .subscribe(
                data => {			
                         this.loading.dismiss();
 let toast = this.toastCtrl.create({
																					message: 'User Sign in sucessful',
																					duration: 3000,
																					cssClass:'toast-container'
																				});
																				toast.present();												 
                         this.nav.setRoot(LoginPage);
                }, 
                  err => {
										let alerts:string= this.parseErrors(err);
										 
										 this.loading.dismiss();	
                                  let toast = this.toastCtrl.create({
																					message: alerts,
																					duration: 3000,
																					cssClass:'toast-container'
																				});
																				toast.present();
                                    console.log(err);
                                });
		}
  }
	//separate method for parsing errors into a single flat array
 parseErrors(response) {
    var errors = '' ;
    for (var key in response.ModelState) {
        for (var i = 0; i < response.ModelState[key].length; i++) {
            errors+= response.ModelState[key][i]+ ",";
        }
    }
    return errors;
}
  
}
