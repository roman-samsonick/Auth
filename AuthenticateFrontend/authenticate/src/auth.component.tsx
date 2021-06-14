import { ChangeEvent, Component } from 'react';
import { Button, Tab, Tabs, TextField } from '@material-ui/core';
import { httpClient } from './axios-instance.constants';
import { appHistory } from './history.utils';
import { setAuthTokens } from 'axios-jwt';
import { set } from 'local-storage';

interface IAuthComponentState {
  email: string;
  password: string;
  name: string;
  isRegister: boolean;
}

export class AuthComponent extends Component<any, IAuthComponentState> {
  private processChange = (property: string) => (event: ChangeEvent<any>) => this.setState({
    [property]: event.target.value,
  } as any);

  private handleTabChange = (event: any, newValue: number) => this.setState({
    isRegister: newValue === 1,
  });

  constructor(props: any) {
    super(props);

    this.state = {
      email: '',
      name: '',
      password: '',
      isRegister: false,
    };
  }

  private createUser = (): void => {
    httpClient.post('user/create-user', this.state)
      .then(() => this.setState({ isRegister: false }));
  };

  private login = (): void => {
    httpClient.post('user/create-token', this.state)
      .then(response => {
        set('user', response.data.user);
        setAuthTokens({
          accessToken: response.data.token,
          refreshToken: '',
        });
      })
      .then(() => appHistory.push('/'));
  };

  render() {
    return (
      <div className="login__container">
        <div className="login__fields">
          <Tabs value={this.state.isRegister ? 1 : 0}
                onChange={this.handleTabChange}>
            <Tab value={0}
                 className="login__tab"
                 label="Login"/>
            <Tab value={1}
                 className="login__tab"
                 label="Register"/>
          </Tabs>

          <TextField variant="outlined"
                     placeholder="Email"
                     onChange={this.processChange('email')}/>

          {
            this.state.isRegister
              ? <TextField variant="outlined"
                           placeholder="Name"
                           onChange={this.processChange('name')}/>
              : null
          }

          <TextField variant="outlined"
                     placeholder="Password"
                     onChange={this.processChange('password')}/>


          <div className="login__button-container">
            <Button onClick={
              this.state.isRegister ? this.createUser : this.login
            }>
              {this.state.isRegister ? 'Register' : 'Login'}
            </Button>
          </div>
        </div>
      </div>
    );
  }
}