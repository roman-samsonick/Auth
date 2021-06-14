import axios from 'axios';
import { clearAuthTokens, getAccessToken } from 'axios-jwt';
import { remove } from 'local-storage';
import { appHistory } from './history.utils';

export const httpClient = axios.create({
  baseURL: process.env.NODE_ENV === 'development' ? 'http://localhost:5000' : 'https://mighty-atoll-31725.herokuapp.com',
});

export const AUTH_FAILED_PATH = '/login';

export const isLogged = ():boolean => !!getAccessToken();

httpClient.interceptors.request.use(requestOptions => {
  return {
    ...requestOptions,
    headers: {
      ...requestOptions.headers,
      Authorization: `Bearer ${getAccessToken()}`,
    },
  };
});

const redirectToLoginOnUnauthorized = (response: { response: { status: number; }; }) => {
  if (response.response && [401, 405, 403].includes(response.response.status)) {
    clearAuthTokens();
    remove('user');
    appHistory.push(AUTH_FAILED_PATH);
  }

  throw response
};

httpClient.interceptors.response.use(r => r, redirectToLoginOnUnauthorized);