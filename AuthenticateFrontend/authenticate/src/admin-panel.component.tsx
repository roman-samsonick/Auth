import { Component } from 'react';
import { DataGrid, GridColumns, GridValueGetterParams } from '@material-ui/data-grid';
import { httpClient } from './axios-instance.constants';
import { Button } from '@material-ui/core';

interface IUser {
  name: string;
  email: string;
  blocked: boolean;
  id: string;
  created: string;
  lastLogin: string;
}

interface IAdminPanelState {
  rows: IUser[];
  selectedUsers: string[];
}

export class AdminPanelComponent extends Component<any, IAdminPanelState> {
  readonly columns: GridColumns = [
    { field: 'id', headerName: 'ID', flex: 1 },
    { field: 'name', headerName: 'Name', flex: 1 },
    { field: 'email', headerName: 'Email', flex: 1 },
    { field: 'created', headerName: 'Created', flex: 1 },
    { field: 'lastLogin', headerName: 'Last login', flex: 1 },
    {
      field: 'blocked',
      headerName: 'Blocked',
      type: 'boolean',
      flex: 1,
    },
    // {
    //   field: 'fullName',
    //   headerName: 'Full name',
    //   description: 'This column has a value getter and is not sortable.',
    //   sortable: false,
    //   width: 160,
    //   valueGetter: (params: GridValueGetterParams) =>
    //     `${params.getValue(params.id, 'firstName') || ''} ${params.getValue(params.id, 'lastName') || ''}`,
    // },
  ];

  constructor(props: any) {
    super(props);

    this.state = {
      rows: [],
      selectedUsers: [],
    };
  }

  componentDidMount() {
    this.initializeRows();
  }

  private initializeRows = () => {
    httpClient.get('user/all').then(response => this.setState({ rows: response.data }));
  }
  
  private updateBlockState = (blocked: boolean) => {
    httpClient.post('user/set-block', { users: this.state.selectedUsers, blocked })
      .then(this.initializeRows);
  }
  
  private deleteUsers = () => {
    httpClient.post('user/delete', { users: this.state.selectedUsers })
      .then(this.initializeRows);
  }

  render() {
    return (
      <div>
        <div>
          <Button onClick={() => this.updateBlockState(true)}>
            Block
          </Button>
          <Button onClick={() => this.updateBlockState(false)}>
            Unblock
          </Button>
          <Button onClick={this.deleteUsers}>
            Delete
          </Button>
        </div>
        <div style={{ height: '30vh', width: '100%' }}>
          <DataGrid rows={this.state.rows}
                    columns={this.columns}
                    pageSize={5}
                    checkboxSelection
                    onSelectionModelChange={
                      selection => this.setState({ selectedUsers: selection.selectionModel as any })
                    }/>
        </div>
      </div>

    );
  }
}

