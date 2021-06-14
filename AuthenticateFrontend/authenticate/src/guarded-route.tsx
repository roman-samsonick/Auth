import React, { Component } from 'react';
import { Route, Redirect } from 'react-router-dom';
import { AUTH_FAILED_PATH } from './axios-instance.constants';

export interface IGuardedRouteProps {
  readonly path: string;
  readonly auth: (props: any) => boolean;
  readonly content: (props: any) => React.ReactNode;
  readonly exact?: boolean;
}

export class GuardedRoute extends Component<IGuardedRouteProps, any> {
  render() {
    return (
      <Route path={this.props.path}
             exact={this.props.exact}
             render={props => this.props.auth(props) ?
               this.props.content(props)
               : <Redirect to={AUTH_FAILED_PATH}/>
             }/>
    );
  }
}
