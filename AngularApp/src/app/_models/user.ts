export class User {
    Id: number;
    Username: string;
    Password: string;
    FirstName: string;
    LastName: string;
    Token: string;

    constructor(username : string, token:string){
        this.Username = username;
        this.Token = token;
    }
}