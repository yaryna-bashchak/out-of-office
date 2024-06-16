import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import NotFound from "../errors/NotFound";
import HomePage from "../../features/home/HomePage";
import ApprovalRequests from "../../features/approvalRequests/ApprovalRequests";
import LeaveRequests from "../../features/leaveRequests/LeaveRequests";
import Projects from "../../features/projects/Projects";
import EmployeeList from "../../features/employees/EmployeeList";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            { path: '', element: <HomePage /> },
            { path: 'employees', element: <EmployeeList /> },
            { path: 'leave-requests', element: <LeaveRequests /> },
            { path: 'approval-requests', element: <ApprovalRequests /> },
            { path: 'projects', element: <Projects /> },
            { path: 'not-found', element: <NotFound /> },
            { path: '*', element: <Navigate replace to='not-found' /> },
        ]
    }
])