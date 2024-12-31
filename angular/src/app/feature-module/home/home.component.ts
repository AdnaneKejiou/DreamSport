import { Component, OnInit } from '@angular/core';
import * as AOS from 'aos';
import { OwlOptions } from 'ngx-owl-carousel-o';
import { routes } from 'src/app/core/helpers/routes';
import { venueCarousel } from 'src/app/shared/model/page.model';

interface data {
  value: string;
}
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  public selectedValue1 = '';
  public selectedValue2 = '';
  public routes = routes;
  category = 'Courts';
  selected = false;
  selectedList1: data[] = [{ value: 'Courts' }, { value: 'Coaches' }];
  selectedList2: data[] = [
    { value: 'Choose Location' },
    { value: 'Germany' },
    { value: 'Russia' },
    { value: 'France' },
    { value: 'UK' },
    { value: 'Colombia' },
  ];
  ngOnInit() {
    AOS.init({ duration: 1200, once: true });
  }
  fav(slide: venueCarousel) {
    slide.favourite = !slide.favourite;
  }
  public topVenueOwlOptions: OwlOptions = {
    margin: 24,
    nav: true,
    loop: true,
    dots: false,
    smartSpeed: 2000,
    navText: [
      '<i class="fas fa-chevron-left"></i>',
      '<i class="fas fa-chevron-right"></i>',
    ],
    responsive: {
      0: {
        items: 1,
      },
      768: {
        items: 3,
      },
      1170: {
        items: 3,
      },
    },
  };
  public coachOwlOptions: OwlOptions = {
    margin: 24,
    nav: true,
    loop: true,
    dots: false,
    smartSpeed: 2000,
    navText: [
      '<i class="fas fa-chevron-left"></i>',
      '<i class="fas fa-chevron-right"></i>',
    ],
    responsive: {
      0: {
        items: 1,
      },
      768: {
        items: 4,
      },
      1170: {
        items: 4,
      },
    },
  };
  public universitiesCompaniesOwlOptions: OwlOptions = {
    loop: true,
    margin: 60,
    nav: false,
    dots: false,
    autoplay: true,
    smartSpeed: 2000,
    navText: [
      "<i class='feather-chevron-left'></i>",
      "<i class='feather-chevron-right'></i>",
    ],
    responsive: {
      0: {
        items: 1,
      },
      500: {
        items: 1,
      },
      768: {
        items: 3,
      },
      1000: {
        items: 5,
      },
    },
  };
  public venueCarouselList: venueCarousel[] = [
    {
      id: 1,
      reviews: '200',
      rating: '4.4',
      badge: 'Featured',
      rate: '453',
      name: 'Mart Sublin',
      title: 'Badminton Academy',
      content:
        'Unleash your badminton potential at our premier ABC Sports Academy, where champions are made.',
      address: 'Sacramento, CA',
      date: '15 May 2023',
      img: 'assets/img/profiles/avatar-01.jpg',
      img2: 'assets/img/venues/venues-01.jpg',
      status: 'active',
      favourite: false,
    },
    {
      id: 2,
      reviews: '450',
      rating: '4.6',
      badge: 'Top Rated',
      rate: '553',
      name: 'Rebecca',
      img: 'assets/img/profiles/avatar-02.jpg',
      img2: 'assets/img/venues/venues-02.jpg',
      title: 'Sarah Sports Academy',
      content:
        'Elevate your athletic journey at Sarah Sports Academy, where excellence meets opportunity.',
      address: 'Port Alsworth, AK',
      date: '18 Jun 2023',
      status: 'inactive',
    },
    {
      id: 3,
      reviews: '112',
      rating: '5.0',
      badge: '',
      rate: '600',
      name: 'Andrew',
      img: 'assets/img/profiles/avatar-03.jpg',
      img2: 'assets/img/venues/venues-03.jpg',
      title: 'Manchester Academy',
      content:
        'Manchester Academy: Where dreams meet excellence in sports education and training.',
      address: 'Guysville, OH',
      date: '17 May 2023',
      status: 'inactive',
    },
    {
      id: 4,
      reviews: '132',
      rating: '3.8',
      badge: 'Featured',
      rate: '650',
      name: 'Andrew Head',
      img: 'assets/img/profiles/avatar-04.jpg',
      img2: 'assets/img/venues/venues-03.jpg',
      title: 'Manchester Academy',
      content:
        'Manchester Academy: Where dreams meet excellence in sports education and training.',
      address: 'Guysville, TH',
      date: '27 May 2023',
      status: 'inactive',
    },
  ];
  coaches = [
    {
      id: 1,
      badge: 'Rookie',
      rate: '250',
      lesson: '4',
      name: 'Kevin Anderson',
      img: 'assets/img/profiles/user-01.jpg',
      favourite: false,
    },
    {
      id: 2,
      badge: 'Intermediate',
      rate: '150',
      lesson: '3',
      name: 'Harry Richardson',
      img: 'assets/img/profiles/user-02.jpg',
      favourite: false,
    },
    {
      id: 3,
      badge: 'Professional',
      rate: '450',
      lesson: '5',
      name: 'Evon Raddick',
      img: 'assets/img/profiles/user-03.jpg',
      favourite: false,
    },
    {
      id: 4,
      badge: 'Rookie',
      rate: '550',
      lesson: '4',
      name: 'Angela Roudrigez',
      img: 'assets/img/profiles/user-04.jpg',
      favourite: false,
    },
    {
      id: 5,
      badge: 'Rookie',
      rate: '250',
      lesson: '4',
      name: 'Kevin Anderson',
      img: 'assets/img/profiles/user-01.jpg',
      favourite: false,
    },
  ];
  courts = [
    {
      id: 1,
      title: 'Smart Shuttlers',
      img: 'assets/img/venues/venues-04.jpg',
      address: '1 Crowthorne Road, 4th Street, NY',
      rating: '4.2',
      review: '300',
      distance: '2.1',
      favourite: false,
    },
    {
      id: 2,
      title: 'Parlers Badminton',
      img: 'assets/img/venues/venues-05.jpg',
      address: 'Hope Street, Battersea, SW11 2DA',
      rating: '4.2',
      review: '200',
      distance: '9.1',
    },
    {
      id: 3,
      title: '6 Feathers',
      img: 'assets/img/venues/venues-06.jpg',
      address: 'Lonsdale Road, Barnes, SW13 9QL',
      rating: '3.2',
      review: '400',
      distance: '10.1',
    },
    {
      id: 4,
      title: 'Parlers Badminton',
      img: 'assets/img/venues/venues-05.jpg',
      address: '1 Crowthorne Road, 4th Street, NY',
      rating: '2.2',
      review: '800',
      distance: '8.1',
    },
  ];
  testimonal = [
    {
      id: 1,
      img: 'assets/img/profiles/avatar-01.jpg',
      name: 'Ariyan Rusov',
      badge: 'Badminton',
      heading: 'Personalized Attention',
      para: "DreamSports' coaching services enhanced my badminton skills. Personalized attention from knowledgeable coaches propelled my game to new heights.",
    },
    {
      id: 2,
      img: 'assets/img/profiles/avatar-04.jpg',
      name: 'Darren Valdez',
      badge: 'Badminton',
      heading: 'Quality Matters !',
      para: "DreamSports' advanced badminton equipment has greatly improved my performance on the court. Their quality range of rackets and shoes made a significant impact.",
    },
    {
      id: 3,
      img: 'assets/img/profiles/avatar-03.jpg',
      name: 'Elinor Dunn',
      badge: 'Badminton',
      heading: 'Excellent Professionalism !',
      para: "DreamSports' unmatched professionalism and service excellence left a positive experience. Highly recommended for court rentals and equipment purchases.",
    },
    {
      id: 4,
      img: 'assets/img/profiles/avatar-04.jpg',
      name: 'Darren Valdez',
      badge: 'Badminton',
      heading: 'Quality Matters !',
      para: "DreamSports' advanced badminton equipment has greatly improved my performance on the court. Their quality range of rackets and shoes made a significant impact.",
    },
  ];
  company = [
    {
      id: 1,
      img: 'assets/img/testimonial-icon-01.svg',
    },
    {
      id: 2,
      img: 'assets/img/testimonial-icon-03.svg',
    },
    {
      id: 3,
      img: 'assets/img/testimonial-icon-04.svg',
    },
    {
      id: 4,
      img: 'assets/img/testimonial-icon-05.svg',
    },
    {
      id: 5,
      img: 'assets/img/testimonial-icon-01.svg',
    },
    {
      id: 6,
      img: 'assets/img/testimonial-icon-04.svg',
    },
  ];
  public badmintonList = [
    {
      title: 'Badminton',
      img1: 'assets/img/venues/venues-07.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'OrlandoWaters',
      year: '15 May 2023',
      para: 'Badminton Gear Guide: Must-Have Equipment for Every Player',
      feedback: '45',
      command: '40',
      img3: 'assets/img/icons/clock.svg',
      time: '10 Min To Read',
      favourite: false,
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/venues/venues-08.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'ClaireNichols',
      year: '16 Jun 2023',
      para: 'Badminton Techniques: Mastering the Smash, Drop Shot, and Clear',
      feedback: '32',
      command: '21',
      img3: 'assets/img/icons/clock.svg',
      time: '5 Min To Read',
      favourite: false,
    },
    {
      title: 'Rules of Game',
      img1: 'assets/img/venues/venues-09.jpg',
      img2: 'assets/img/profiles/avatar-06.jpg',
      name: 'JoannaLe',
      year: '17 May 2023',
      para: 'The Evolution of Badminton:From Backyard Fun to Olympic Sport',
      feedback: '55',
      command: '32',
      img3: 'assets/img/icons/clock.svg',
      time: '4 Min To Read',
      favourite: false,
    },
    {
      title: 'Badminton',
      img1: 'assets/img/venues/venues-15.jpg',
      img2: 'assets/img/profiles/avatar-02.jpg',
      name: 'Andrew',
      year: '17 May 2023',
      para: 'Sports Make Us A Lot Stronger And Healthier Than We Think',
      feedback: '55',
      command: '32',
      img3: 'assets/img/icons/clock.svg',
      time: '4 Min To Read',
      favourite: false,
    },
    {
      title: 'Rules of Game',
      img1: 'assets/img/venues/venues-16.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'MartSublin',
      year: '16 Jun 2023',
      para: 'Sports Make Us A Lot Stronger And Healthier Than We Think',
      feedback: '32',
      command: '21',
      img3: 'assets/img/icons/clock.svg',
      time: '5 Min To Read',
      favourite: false,
    },
    {
      title: 'Sports Activites',
      img1: 'assets/img/venues/venues-17.jpg',
      img2: 'assets/img/profiles/avatar-01.jpg',
      name: 'Rebecca',
      year: '15 May 2023',
      para: 'Badminton Gear Guide: Must-Have Equipment for Every Player',
      feedback: '45',
      command: '40',
      img3: 'assets/img/icons/clock.svg',
      time: '10 Min To Read',
      favourite: false,
    },
  ];
}
