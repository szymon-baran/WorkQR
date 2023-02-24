export interface Todo {
  id: number;
  content: string;
}

export interface Meta {
  totalCount: number;
}

export interface UserDTO {
  username: string;
  token: string;
  refreshToken: string;
  expiration: bigint;
  roles: Array<string>;
}
