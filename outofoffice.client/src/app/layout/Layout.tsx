import { Outlet, Link, useNavigate } from 'react-router-dom';
import './Layout.css';
import { useContext } from 'react';
import UserRoleContext, { UserRole, roles } from '../context/UserRoleContext';

interface SidebarLink {
    link: string;
    label: string;
}

const sidebarLinks: SidebarLink[] = [
    { link: '/employees', label: 'Employees' },
    { link: '/leave-requests', label: 'Leave Requests' },
    { link: '/approval-requests', label: 'Approval Requests' },
    { link: '/projects', label: 'Projects' },
];

const Layout = () => {
    const navigate = useNavigate();
    const userRoleContext = useContext(UserRoleContext);

    if (!userRoleContext) {
        throw new Error('Layout must be used within a UserRoleProvider');
    }

    const { userRole, setUserRole } = userRoleContext;

    const handleRoleChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setUserRole(event.target.value as UserRole);
        navigate('/');
    };

    const filteredSidebarLinks = userRole === 'Employee'
        ? sidebarLinks.filter(link => link.link === '/leave-requests')
        : sidebarLinks;

    return (
        <div className="layout">
            <nav className="sidebar">
                <div className="role-switcher">
                    <label htmlFor="role-select">Select Role:</label>
                    <select id="role-select" value={userRole} onChange={handleRoleChange}>
                        {roles.map((role) => (
                            <option value={role}>{role}</option>
                        ))}
                    </select>
                </div>
                <ul>
                    {filteredSidebarLinks.map((sidebarLink) => (
                        <li><Link to={sidebarLink.link}>{sidebarLink.label}</Link></li>
                    ))}
                </ul>
            </nav>
            <main className="content">
                <Outlet />
            </main>
        </div>
    );
};

export default Layout;
