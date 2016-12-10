import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import 'rxjs/add/operator/map'; 
import {HttpClient} from './HttpClient';
import {Observable} from 'rxjs/Rx';

@Injectable()
export class ValuesService {
    constructor(private http: HttpClient) { }

    getAll() {
        return this.http.get('http://localhost:53852/api/UserInfo').map((response: Response) => response.json());
    }

    getValues() {
        return this.http.get('http://localhost:53852/api/Values').map((response: Response) => response.json());
    }
		
		  UpdateAddress(Address:any) {
        return this.http.put('http://localhost:53852/api/Addresses/'+Address.Id,Address).map((response: Response) => response.json())
				 .catch((error:any) => Observable.throw(error || 'Server error'));
    }
		InsertAddress(Address:any) {
        return this.http.post('http://localhost:53852/api/Addresses/',Address).map((response: Response) => response.json())
				 .catch((error:any) => Observable.throw(error || 'Server error'));;
    }
		Register(user:any) {
        return this.http.post('http://localhost:53852/api/Account/Register',user).map((response: Response) => response.json())
				  .catch((error:any) => Observable.throw(error.json() || 'Server error'));

     
    }
}