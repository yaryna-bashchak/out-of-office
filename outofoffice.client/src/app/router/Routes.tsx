import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import NotFound from "../errors/NotFound";
import HomePage from "../../features/home/HomePage";
import Employees from "../../features/employees/Employees";
import ApprovalRequests from "../../features/approvalRequests/ApprovalRequests";
import LeaveRequests from "../../features/leaveRequests/LeaveRequests";
import Projects from "../../features/projects/Projects";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            { path: '', element: <HomePage /> },
            { path: 'employees', element: <Employees /> },
            { path: 'leave-requests', element: <LeaveRequests /> },
            { path: 'approval-requests', element: <ApprovalRequests /> },
            { path: 'projects', element: <Projects /> },
            { path: 'not-found', element: <NotFound /> },
            { path: '*', element: <Navigate replace to='not-found' /> },
        ]
    }
])