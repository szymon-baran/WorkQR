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
