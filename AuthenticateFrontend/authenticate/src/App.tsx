import React, { Component } from 'react';
import './App.css';
import { isLogged } from './axios-instance.constants';
import { Route, Router } from 'react-router-dom';
import { appHistory } from './history.utils';
import { GuardedRoute } from './guarded-route';
import { AdminPanelComponent } from './admin-panel.component';
import { AuthComponent } from './auth.component';

export class Application extends Component<any, any> {
  render() {
    return (
      <Router history={appHistory}>
        <Route path="/login" 
               component={AuthComponent}/>
        <GuardedRoute path="/" 
                      exact={true}
                      auth={isLogged}
                      content={() => <AdminPanelComponent/>}/>
      </Router>
    );
  }

  componentDidMount(): void {
    // const email = 'johnsutray@gmail.com';
    // const password = 'Qwe_123';
    //
    // httpClient.post('user/create-user', {
    //   name: 'John Sutray',
    //   email,
    //   password,
    // }).then(() => {
    //   httpClient.post('user/create-token', { email, password }).then((result) => {
    //     setAuthTokens({
    //       accessToken: result.data.token,
    //       refreshToken: '',
    //     });
    //    
    //     httpClient.get('user/all').then(result => {
    //       httpClient.post('user/set-block', { id: result.data[0].id, blocked: true }).then(() => {
    //         httpClient.get('user/all').then(console.log);
    //       });
    //     });
    //   });
    // });
  }
}
