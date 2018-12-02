export class User {
    Id: number;
    Username: string;
    Password: string;
    FirstName: string;
    LastName: string;
    Token: string;
    Admin: boolean;

    constructor(username : string, token:string, admin: boolean){
        this.Username = username;
        this.Token = token;
        this.Admin = admin;
    }
}