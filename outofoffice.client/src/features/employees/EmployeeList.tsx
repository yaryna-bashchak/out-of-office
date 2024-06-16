import { useContext } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';

const EmployeeList = () => {
    const context = useContext(EmployeeContext);

    if (!context) {
        throw new Error('EmployeeList must be used within an EmployeeProvider');
    }

    const { employees } = context;

    return (
        <div>
            <h1>Employees</h1>
            <ul>
                {employees.map(emp => (
                    <li key={emp.id}>
                        {emp.fullName} - {emp.position.name}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default EmployeeList;
