import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.


export interface Employee {
    id: number;
    employeeId: string;
    employeeName?: string;
    gender?: string;
    city?: string;
    department?: string;
}

export interface APIResponse {
    status: string;
    errorcode?: string;
    data?: [];
    message?: string;
    department?: string;
}


export interface EmployeeState {
    isLoading?: boolean;
    startDateIndex?: number;
    employees?: Employee[];
    Model(): void;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestEmployeeAction {
    type: 'REQUEST_EMPLOYEE_DATA';
    startDateIndex: number;
}

interface ReceiveEmployeeAction {
    type: 'RECEIVE_EMPLOYEE_DATA';
    startDateIndex: number;
    employees: Employee[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestEmployeeAction | ReceiveEmployeeAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestEmployees: (startDateIndex: number): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        if (appState && appState.employees && startDateIndex !== appState.employees.startDateIndex) {
            fetch(`api/employee`)
                .then(response => response.json() as Promise<Employee[]>)
                .then(data => {
                    dispatch({ type: 'RECEIVE_EMPLOYEE_DATA', startDateIndex: startDateIndex, employees: data });
                });

            dispatch({ type: 'REQUEST_EMPLOYEE_DATA', startDateIndex: startDateIndex });
        }
    }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.

const unloadedState: EmployeeState = { employees: [], isLoading: false };

export const reducer: Reducer<EmployeeState> = (state: EmployeeState | undefined, incomingAction: Action): EmployeeState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_EMPLOYEE_DATA':
            return {
                startDateIndex: action.startDateIndex,
                employees: state.employees,
                isLoading: true
            };
        case 'RECEIVE_EMPLOYEE_DATA':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startDateIndex === state.startDateIndex) {
                return {
                    startDateIndex: action.startDateIndex,
                    employees: action.employees,
                    isLoading: false
                };
            }
            break;
    }

    return state;
};
