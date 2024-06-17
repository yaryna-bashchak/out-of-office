import { Navigate, createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import NotFound from "../errors/NotFound";
import HomePage from "../../features/home/HomePage";
import EmployeeList from "../../features/employees/EmployeeList";
import EmployeeEditor from "../../features/employees/EmployeeEditor";
import EmployeeDetails from "../../features/employees/EmployeeDetails";
import LeaveRequestList from "../../features/leaveRequests/LeaveRequestList";
import LeaveRequestDetails from "../../features/leaveRequests/LeaveRequestDetails";
import LeaveRequestEditor from "../../features/leaveRequests/LeaveRequestEditor";
import ApprovalRequestList from "../../features/approvalRequests/ApprovalRequestList";
import ApprovalRequestDetails from "../../features/approvalRequests/ApprovalRequestDetails";
import LeaveRejectionComment from "../../features/approvalRequests/LeaveRejectionComment";
import ProjectList from "../../features/projects/ProjectList";
import ProjectEditor from "../../features/projects/ProjectEditor";
import ProjectDetails from "../../features/projects/ProjectDetails";

export const router = createBrowserRouter([
    {
        path: '/',
        element: <App />,
        children: [
            { path: '', element: <HomePage /> },
            { path: 'employees', element: <EmployeeList /> },
            { path: 'employees/:id', element: <EmployeeDetails /> },
            { path: 'employees/:id/edit', element: <EmployeeEditor /> },
            { path: 'employees/new', element: <EmployeeEditor /> },
            { path: 'leave-requests', element: <LeaveRequestList /> },
            { path: 'leave-requests/:id', element: <LeaveRequestDetails /> },
            { path: 'leave-requests/:id/edit', element: <LeaveRequestEditor /> },
            { path: 'leave-requests/new', element: <LeaveRequestEditor /> },
            { path: 'approval-requests', element: <ApprovalRequestList /> },
            { path: 'approval-requests/:id', element: <ApprovalRequestDetails /> },
            { path: 'approval-requests/:id/reject', element: <LeaveRejectionComment /> },
            { path: 'projects', element: <ProjectList /> },
            { path: 'projects/:id', element: <ProjectDetails /> },
            { path: 'projects/:id/edit', element: <ProjectEditor /> },
            { path: 'projects/new', element: <ProjectEditor /> },
            { path: 'not-found', element: <NotFound /> },
            { path: '*', element: <Navigate replace to='not-found' /> },
        ]
    }
])