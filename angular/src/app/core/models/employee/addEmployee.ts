export interface AddEmployee {
    nom: string;
    prenom: string;
    phoneNumber: string;
    cin: string;
    email: string;
    birthday: string | Date;  // Can be string or Date object
    username: string;
    salaire: number;
  }