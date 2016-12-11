import {Component} from '@angular/core';
import {Platform} from 'ionic-angular';
import {ViewChild} from '@angular/core';
import {StatusBar} from 'ionic-native';
// import pages
import {HomePage} from '../pages/home/home';
import {CategoriesPage} from '../pages/categories/categories';
import {FavoritePage} from '../pages/favorite/favorite';
import {CartPage} from '../pages/cart/cart';
import {UserPage} from '../pages/user/user';
import {SettingPage} from '../pages/setting/setting';
import {AboutPage} from '../pages/about/about';
import {LoginPage} from '../pages/login/login';
import {ChatsPage} from '../pages/chats/chats';
import {AddressPage} from '../pages/address/address';
// end import pages

@Component({
  templateUrl: 'app.html',
  queries: {
    nav: new ViewChild('content')
  }
})
export class MyApp {

  public rootPage: any;

  public nav: any;

  public pages = [
    {
      title: 'Home',
      icon: 'ios-home-outline',
      count: 0,
      component: HomePage
    },

    {
      title: 'Products',
      icon: 'apps',
      count: 0,
      component: CategoriesPage
    },

    {
      title: 'Favorite',
      icon: 'star-outline',
      count: 0,
      component: FavoritePage
    },

    {
      title: 'My Cart',
      icon: 'ios-cart-outline',
      count: 0,
      component: CartPage
    },

    {
      title: 'Setting',
      icon: 'ios-settings-outline',
      count: 0,
      component: SettingPage
    },
		{
      title: 'Addresses',
      icon: 'ios-heart-outline',
      count: 0,
      component: AddressPage
    },
    {
      title: 'About us',
      icon: 'ios-information-circle-outline',
      count: 0,
      component: AboutPage
    },

    {
      title: 'Supports',
      icon: 'ios-help-circle-outline',
      count: 0,
      component: ChatsPage
    },

    {
      title: 'Logout',
      icon: 'ios-exit-outline',
      count: 0,
      component: LoginPage
    },
    // import menu


  ];

  constructor(public platform: Platform) {
    this.rootPage = HomePage;

    platform.ready().then(() => {
      // Okay, so the platform is ready and our plugins are available.
      // Here you can do any higher level native things you might need.
      StatusBar.styleDefault();
    });
  }

  openPage(page) {
    // Reset the content nav to have just this page
    // we wouldn't want the back button to show in this scenario
    this.nav.setRoot(page.component);
  }

  // view my profile
  viewMyProfile() {
    this.nav.setRoot(UserPage);
  }
}


