import * as React from 'react';
import { RouteComponentProps, StaticContext } from 'react-router';
import { Link, NavLink, useParams} from 'react-router-dom';
import * as EmployeeStore from '../store/Employee';
import { ApplicationState } from '../store';
import { connect } from 'react-redux';
import { DomainConverter } from '../helper/DomainHelper';

type EmployeeProps =
    EmployeeStore.EmployeeState // ... state we've requested from the Redux store
    & typeof EmployeeStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ startDateIndex: string }> // ... plus incoming routing parameters
    & RouteComponentProps<{ mode: string }>

type Props<EmployeeProps> =
    EmployeeStore.EmployeeState // ... state we've requested from the Redux store
    & typeof EmployeeStore.actionCreators // ... plus action creators we've requested
    & RouteComponentProps<{ startDateIndex: string }> // ... plus incoming routing parameters
    & RouteComponentProps<{ mode: string }>

class Employee implements EmployeeStore.Employee {
    id=0;
    employeeId= "";
    employeeName? = "";
    gender?= "";
    city?= "";
    department?="";
}
interface FetchEmployeeDataState {
    empList: Employee,
    loading: boolean,
    title: string,
    cityList: [],
}


export class AddEmployee extends React.Component<Props<EmployeeProps>, FetchEmployeeDataState> {
    

    constructor(props: Props<EmployeeProps>) {
        super(props);
       
        this.state = {
            title: "", loading: true, empList: new Employee, cityList:[]
        };

        fetch('employee/cities')
            .then(response => response.json())
            .then(cities => {
                this.setState({ cityList: cities.data });
            });

        //fetch('api/Employee/GetCityList')
        //    .then(response => response.json() as Promise<Array<any>>)
        //    .then(data => {
        //        this.setState({ cityList: data });
        //    });
        const code:any = props.match.params;
        const empid:string = code.empid==undefined?0:code.empid;
        // This will set state for Edit employee
        if (code.empid > 0) {
            fetch('employee/detail/' + empid)
                .then(response => response.json() as Promise<EmployeeStore.Employee>)
                .then(data => {
                    this.setState({
                        title: "Edit", loading: false, empList: data});
                });
        }

        // This will set state for Add employee
        else {
            this.state = {
                title: "", loading: false, empList: new Employee, cityList: this.state.cityList
            };

        }

        // This binding is necessary to make "this" work in the callback
        this.handleSave = this.handleSave.bind(this);
        this.handleCancel = this.handleCancel.bind(this);
    }

    public render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderCreateForm();

        return <div>
            <h1>{this.state.title}</h1>
            <h3>Employee</h3>
            <hr />
            {contents}
        </div>;
    }

    // This will handle the submit form event.
    private handleSave(event:any) {
        console.log(event);
        event.preventDefault();
        const formData = new FormData(event.target);
        
        const payload = DomainConverter.getPayload<Employee>(new Employee, formData);

        // PUT request for Edit employee.
        if (this.state.empList.id > 0) {
            fetch('employee/update', {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),

            }).then((response) => response.json())
                .then((responseJson) => {
                    this.props.history.push("/employee");
                })
        }
        // POST request for Add employee.
        else {
            payload.id = 0;
            fetch('employee/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload),

            }).then((response) => response.json())
                .then((responseJson) => {
                    this.props.history.push("/employee");
                })
        }
    }

    // This will handle Cancel button click event.
    private handleCancel(e: any) {
        e.preventDefault();
        this.props.history.push("/employee");
    }

    // Returns the HTML Form to the render() method.
    private renderCreateForm() {
        return (
            <form onSubmit={this.handleSave} >
                <input type="text" hidden name="id" defaultValue={this.state.empList.id} />

                <div className="form-group row" >
                    <label className=" control-label col-md-12" htmlFor="EmployeeId">Employee Id</label>
                    <div className="col-md-4">
                        <input className="form-control" type="text" contentEditable={false} name="employeeId" defaultValue={this.state.empList.employeeId ? this.state.empList.employeeId.trim():""} />
                    </div>
                </div>
                < div className="form-group row" >
                    <label className=" control-label col-md-12" htmlFor="employeeName">Name</label>
                    <div className="col-md-4">
                        <input className="form-control" type="text" name="employeeName" defaultValue={this.state.empList.employeeName?this.state.empList.employeeName.trim():""} required />
                    </div>
                </div >
                <div className="form-group row">
                    <label className="control-label col-md-12" htmlFor="Gender">Gender</label>
                    <div className="col-md-4">
                        <select className="form-control" data-val="true" name="gender" defaultValue={this.state.empList.gender? this.state.empList.gender.trim():""} required>
                            <option value="">-- Select Gender --</option>
                            <option value="Male">Male</option>
                            <option value="Female">Female</option>
                        </select>
                    </div>
                </div >
                <div className="form-group row">
                    <label className="control-label col-md-12" htmlFor="Department" >Department</label>
                    <div className="col-md-4">
                        <input className="form-control" type="text" name="department" defaultValue={this.state.empList.department ? this.state.empList.department.trim():""} required />
                    </div>
                </div>
                <div className="form-group row">
                    <label className="control-label col-md-12" htmlFor="City">City</label>
                    <div className="col-md-4">
                        <select className="form-control" data-val="true" name="city" defaultValue={this.state.empList.city ? this.state.empList.city.trim():""} required>
                            <option value="">-- Select City --</option>
                            {this.state.cityList.map((city : any) =>
                                <option key={city.name} value={city.Name}>{city.name}</option>
                            )}
                        </select>
                    </div>
                </div >
                <div className="form-group">
                    <button type="submit" className="btn btn-default">Save</button>
                    <button className="btn" onClick={this.handleCancel}>Cancel</button>
                </div >
            </form >
        )
    }
}

export default connect(
    (state: ApplicationState) => state.employees, // Selects which state properties are merged into the component's props
    EmployeeStore.actionCreators // Selects which action creators are merged into the component's props
)(AddEmployee as any); // eslint-disable-line @typescript-eslint/no-explicit-any
