export interface UserDTO {
  username: string;
  firstName: string;
  lastName: string;
  token: string;
  refreshToken: string;
  expiration: bigint;
  roles: Array<string>;
}

export interface UserTokenDTO {
  accessToken: string;
  refreshToken: string;
  expiration: bigint;
}

export interface CompanyRegisterResultDTO {
  moderatorResult: object;
  scannerResult: object;
  scannerUsername: string;
  scannerPassword: string;
}

export interface EventScanDTO {
  id: string;
  fullName: string;
  eventType: number;
  breakMinutesLeftToday: number;
}

const date = new Date();
date.setMonth(date.getMonth() - 1);

export class GetUserDetailsVM {
  UserId: string;
  DateFrom: string;
  DateTo: string;
  Description: string;

  constructor() {
    this.UserId = '';
    this.DateFrom = date.toISOString().slice(0, 10);
    this.DateTo = new Date().toISOString().slice(0, 10);
    this.Description = '';
  }
}
