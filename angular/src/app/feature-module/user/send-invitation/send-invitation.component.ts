import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { routes } from 'src/app/core/core.index';
import { UserService, User } from 'src/app/core/service/user/user.service';

@Component({
  selector: 'app-send-invitation',
  templateUrl: './send-invitation.component.html',
  styleUrls: ['./send-invitation.component.scss']
})
export class SendInvitationComponent {
  public routes = routes;
  public searchDataValue = '';
  public filteredUsers: User[] = [];
  public isLoading = false;
  private searchSubject = new Subject<string>();

  constructor(
    private router: Router,
    private userService: UserService
  ) {
    this.setupSearchDebounce();
  }

  private setupSearchDebounce(): void {
    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(searchValue => {
      this.searchUsers(searchValue);
    });
  }

  onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  searchUsers(searchTerm: string): void {
    const trimmedTerm = searchTerm.trim();
    
    if (trimmedTerm === '') {
      this.filteredUsers = [];
      return;
    }

    this.isLoading = true;
    
    // Encode le terme de recherche pour les espaces
    const encodedSearchTerm = encodeURIComponent(trimmedTerm);
    
    this.userService.searchUsers(encodedSearchTerm).subscribe({
      next: (users) => {
        // Filtrage supplémentaire côté client si nécessaire
        if (trimmedTerm.includes(' ')) {
          const terms = trimmedTerm.toLowerCase().split(' ');
          this.filteredUsers = users.filter(user => 
            this.matchesAllTerms(user, terms)
          );
        } else {
          this.filteredUsers = users;
        }
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.filteredUsers = [];
      }
    });
  }

  private matchesAllTerms(user: User, terms: string[]): boolean {
    const userFullName = `${user.prenom} ${user.nom}`.toLowerCase();
    const userReverseName = `${user.nom} ${user.prenom}`.toLowerCase();
    const searchPattern = terms.join(' ').toLowerCase();

    return userFullName.includes(searchPattern) || 
           userReverseName.includes(searchPattern) ||
           terms.every(term =>
             user.nom.toLowerCase().includes(term) ||
             user.prenom.toLowerCase().includes(term) ||
             (user.username && user.username.toLowerCase().includes(term)) ||
             (user.bio && user.bio.toLowerCase().includes(term))
           );
  }

  sendInvitation(user: User): void {
    this.userService.sendInvitation(user.id).subscribe({
      next: () => {
        console.log(`Invitation envoyée à ${user.nom} ${user.prenom}`);
        this.filteredUsers = this.filteredUsers.filter(u => u.id !== user.id);
      },
      error: (err) => {
        console.error('Erreur lors de l\'envoi de l\'invitation', err);
      }
    });
  }
}