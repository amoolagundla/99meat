import { Component,OnInit } from '@angular/core';
import { NavController } from 'ionic-angular';
import {Address,UserInfo} from '../../app/app.module';
import { ValuesService } from '../../services/ValuesService';
/*
  Generated class for the Address page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/
@Component({
  selector: 'page-address',
  templateUrl: 'address.html'
})
export class AddressPage  implements OnInit {
	public userInfo :UserInfo;
public myVar:boolean;
public address:any={
				Address1:'',
				Address2:'',
				State:'',
				City:'',
				Country:'India',
				Address3:'',
				UserName:'',
				Id:0,
				IsDefault:false,
				PostalCode:''
				};
	ngOnInit() {
		
		
  }
	

  constructor(public navCtrl: NavController,public valuesService:ValuesService) {
			let currentUser = 	localStorage.getItem('UserInfo');
			if(currentUser!=null)
			{
				this.userInfo =  SerializationHelper.toInstance(new UserInfo(), currentUser);
				console.log(this.userInfo);
				if(this.userInfo.Addresses.length==0)
			{
				
					
				this.myVar=true;
			}
			else
			{
			this.myVar=false;
			}
			}	
			
	}
EditAddress(Addresses:any)
{
	this.address=Addresses;
	this.myVar=!this.myVar;
}
remove(Id:any)
{
}
	
	 save(model: Address, isValid: boolean) {
   console.log(model);
		if(isValid)
		{
			if(model.Id>0)
			{
			this.valuesService.UpdateAddress(model)
            .subscribe(
                da => {this.myVar=false;});
			}
			else{
				this.valuesService.InsertAddress(model)
            .subscribe(
                da => {this.myVar=false;});
			}
		}
  }

}
class SerializationHelper {
    static toInstance<T>(obj: T, json: string) : T {
        var jsonObj = JSON.parse(json);

        if (typeof obj["fromJSON"] === "function") {
            obj["fromJSON"](jsonObj);
        }
        else {
            for (var propName in jsonObj) {
                obj[propName] = jsonObj[propName]
            }
        }

        return obj;
    }
}