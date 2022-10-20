import * as React from 'react';
import { RouteComponentProps, StaticContext } from 'react-router';
import { Link, NavLink } from 'react-router-dom';
import * as EmployeeStore from '../store/Employee';
import { ApplicationState } from '../store';
import { connect } from 'react-redux';

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
    title:string,
}

export class FetchEmployee extends React.Component<Props<EmployeeProps>, FetchEmployeeDataState>  {

    constructor(props: Props<EmployeeProps>) {
        super(props);
        this.state = { empList:[],title:"name", loading: true };

        fetch('employee')
            .then(response => response.json() as Promise<EmployeeStore.APIResponse>)
            .then(data => {
                this.setState({ empList: data.data as EmployeeStore.Employee[] , loading: false });
            });

        // This binding is necessary to make "this" work in the callback
        this.handleDelete = this.handleDelete.bind(this);
        this.handleEdit = this.handleEdit.bind(this);

    }
    private mode: boolean = false;
        public componentDidMount() {
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderEmployeeTable();

        return <div>
            <h1>Employee Data</h1>
            <p>This component demonstrates fetching Employee data from the server.</p>
            <p>
                <Link to="/addemployee">Create New</Link>
            </p>
            {contents}
        </div>;
    }

    // Handle Delete request for an employee
    private handleDelete(id: string) {
        if (!confirm("Do you want to delete employee with Id: " + id))
            return;
        else {
            fetch('api/Employee/Delete/' + id, {
                method: 'delete'
            }).then(data => {
                this.setState(
                    {
                        empList: this.state.empList.filter((rec) => {
                            return (rec.employeeId != id);
                        })
                    });
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
                    <th>EmployeeId</th>
                    <th>Name</th>
                    <th>Gender</th>
                    <th>Department</th>
                    <th>City</th>
                </tr>
            </thead>
            <tbody>
                {this.state.empList.map((emp: EmployeeStore.Employee) =>
                    <tr key={emp.employeeId}>
                        <td></td>
                        <td>{emp.employeeId}</td>
                        <td>{emp.employeeName}</td>
                        <td>{emp.gender}</td>
                        <td>{emp.department}</td>
                        <td>{emp.city}</td>
                        <td>
                            <a className="action" onClick={(id) => this.handleEdit(emp.employeeId)}>Edit</a>  |
                            <a className="action" onClick={(id) => this.handleDelete(emp.employeeId)}>Delete</a>
                        </td>
                    </tr>
                )}
            </tbody>
        </table>;
    }
}

export default connect(
    (state: ApplicationState) => state.employees, // Selects which state properties are merged into the component's props
    EmployeeStore.actionCreators // Selects which action creators are merged into the component's props
)(FetchEmployee as any); // eslint-disable-line @typescript-eslint/no-explicit-any
