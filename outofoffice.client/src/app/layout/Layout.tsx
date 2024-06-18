import { Outlet, Link } from 'react-router-dom';
import './Layout.css';
import { useContext } from 'react';
import UserRoleContext from '../context/UserRoleContext';

const Layout = () => {
    const userRoleContext = useContext(UserRoleContext);

    if (!userRoleContext) {
        throw new Error('Layout must be used within a UserRoleProvider');
    }

    const { userRole, setUserRole } = userRoleContext;

    const handleRoleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setUserRole(event.target.value as 'HR Manager' | 'Project Manager' | 'Employee');
    };

    return (
        <div className="layout">
            <nav className="sidebar">
                <div className="role-switcher">
                    <label htmlFor="role-select">Select Role:</label>
                    <select id="role-select" value={userRole} onChange={handleRoleChange}>
                        <option value="HR Manager">HR Manager</option>
                        <option value="Project Manager">Project Manager</option>
                        <option value="Employee">Employee</option>
                    </select>
                </div>
                <ul>
                    <li><Link to="/employees">Employees</Link></li>
                    <li><Link to="/leave-requests">Leave Requests</Link></li>
                    <li><Link to="/approval-requests">Approval Requests</Link></li>
                    <li><Link to="/projects">Projects</Link></li>
                </ul>
            </nav>
            <main className="content">
                <Outlet />
            </main>
        </div>
    );
};

export default Layout;
