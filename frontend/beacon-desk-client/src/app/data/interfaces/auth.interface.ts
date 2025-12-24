export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponseData {
  token: string;
}

export interface UserTokenPayload{
  nameId: string;
  unique_name: string;
  role: string[];
  nbf: number;
  exp: number;
  iat: number;
  iss: number;
}


export interface AuthUser {
  id: string;
  email: string;
  role: string;
}