import * as React from 'react';
import { RouteComponentProps, StaticContext } from 'react-router';
import { Link, NavLink, useParams } from 'react-router-dom';
import * as EmployeeStore from '../store/Employee';
import { ApplicationState } from '../store';
import { connect } from 'react-redux';
import ModalComponent from './Model_Popup';
import { useState } from 'react';
import queryString from 'query-string'


useState

type EmployeeProps =
    EmployeeStore.EmployeeState // ... state we've requested from the Redux store
    & typeof EmployeeStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ startDateIndex: string }> // ... plus incoming routing parameters
    & RouteComponentProps<{ mode: string }>

type Props<EmployeeProps>  =  
     EmployeeStore.EmployeeState // ... state we've requested from the Redux store
    & typeof EmployeeStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps < { startDateIndex: string } > // ... plus incoming routing parameters
    & RouteComponentProps<{ mode: string }>

interface FetchEmployeeDataState {
    empList: EmployeeStore.Employee[],
    loading: boolean,
    title: string,
    showModel: boolean,
}

export class FetchEmployee extends React.Component<Props<EmployeeProps>, FetchEmployeeDataState>  {
    constructor(props: Props<EmployeeProps>) {
        super(props);
        this.state = { empList:[],title:"name", loading: true , showModel:false};

        this.getEmployees(0);

        // This binding is necessary to make "this" work in the callback
        this.handleDelete = this.handleDelete.bind(this);
        this.handleEdit = this.handleEdit.bind(this);
    }
   
    private ensureDataFetched() {

        const search = this.props.location.search; // could be '?foo=bar'
        const params: any = queryString.parse(search);
        let nextStartDateIndex = params?.startPage == undefined ? 1 : parseInt(params.startPage) + 1; // bar
        this.getEmployees(nextStartDateIndex);
    }
    getEmployees(nextStartDateIndex:number) {
        this.setState({
            loading: true
        });
        fetch(`api/employee?startPage=${nextStartDateIndex}`)
            .then(response => response.json() as Promise<EmployeeStore.APIResponse>)
            .then(data => {
                this.setState({ loading: false, empList: data.data as EmployeeStore.Employee[] })
            });
    }
    reload() {
        //window.location.reload();
    }
    handleClick = () => {
    }
    private mode: boolean = false;
    public componentDidMount() {
        this.ensureDataFetched();

    }
    handleModal() {
        this.setState({ showModel: !this.state.showModel })
    }

    

     public render() {
         let contents = this.state.loading
             ? <p><em>Loading...</em></p>
             : this.renderEmployeeTable() 

        return <div>
            <h1>Employee Data</h1>
            <p>This component demonstrates fetching Employee data from the server.</p>
            <p>
                <Link to="/addemployee">Create New</Link>
                <ModalComponent RenderEmployees={this.reload}/>
            </p>
            { contents }
        </div>;
    }

    // Handle Delete request for an employee
    private handleDelete(id: string) {
        if (!confirm("Do you want to delete employee with Id: " + id))
            return;
        else {
            fetch('api/employee/delete/' + id.toString(), {
                method: 'Delete'
            }).then(data => {
                this.getEmployees(0);
            });
        }
    }

    private handleEdit(id: string) {
        this.props.history.push("/editemployee/edit/" + id);
    }

    // Returns the HTML table to the render() method.
    private renderEmployeeTable() {
        return <table className='table'>
            <thead>
                <tr>
                    <th></th>
                    <th><a href="http://localhost:5263/download/zip">Download Zip</a></th>
                    <th>EmployeeId</th>
                    <th>Name</th>
                    <th>Gender</th>
                    <th>Department</th>
                    <th>City</th>
                </tr>
            </thead>
            <tbody>
                {this.state.empList.map((emp: EmployeeStore.Employee) =>
                    <tr key={emp.id.toString()}>
                        <td></td>
                        <td><input type="checkbox" value={emp.id}/></td>
                        <td>{emp.employeeId}</td>
                        <td><a href={emp.downloadUrl}>{emp.employeeName}</a></td>
                        <td>{emp.gender}</td>
                        <td>{emp.department}</td>
                        <td>{emp.city}</td>
                        <td>
                            <a className="action" onClick={(id) => this.handleEdit(emp.id.toString())}>Edit</a>  |
                            <a className="action" onClick={(id) => this.handleDelete(emp.id.toString())}>Delete</a>
                        </td>
                    </tr>
                )}
            </tbody>
        </table>;
    }
    private renderPagination() {

        const search = this.props.location.search; // could be '?foo=bar'
        const params: any = queryString.parse(search);
        let nextStartDateIndex = params?.startPage == undefined ? 1 : parseInt(params.startPage) + 1; // bar
       
        return (
            <div className="d-flex justify-content-between">
                <Link className='btn btn-outline-secondary btn-sm' to={`/employee?startPage=${parseInt(params.startPage) == 1 ? 1 :( parseInt(params.startPage) - 1)}`}>Previous</Link>
                {this.props.isLoading && <span>Loading...</span>}
                <Link className='btn btn-outline-secondary btn-sm' to={`/employee?startPage=${nextStartDateIndex}`}>Next</Link>
            </div>
        );
    }
}

export default connect(
    (state: ApplicationState) => state.employees, // Selects which state properties are merged into the component's props
    EmployeeStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchEmployee as any); // eslint-disable-line @typescript-eslint/no-explicit-any
