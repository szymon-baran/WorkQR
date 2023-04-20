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

export class GetEventsVM {
  UserId: string;
  DateFrom: string;
  DateTo: string;
  Description: string;
  EventType: number;

  constructor() {
    this.UserId = '';
    this.DateFrom = new Date().toISOString().slice(0, 10);
    this.DateTo = new Date().toISOString().slice(0, 10);
    this.Description = '';
    this.EventType = 0;
  }
}
