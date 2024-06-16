import { Outlet, Link } from 'react-router-dom';
import './Layout.css';

const Layout = () => {
    return (
        <div className="layout">
            <nav className="sidebar">
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
