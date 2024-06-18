import { useContext } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { Employee } from '../../app/models/employee';
import { SortConfig, useSortableData } from '../../app/hooks/useSortableData';
import SortableTableCell from '../../app/components/SortableTableCell';
import { EditButton, ViewButton } from '../../app/components/ListButtons';
import BoldTableCell from "../../app/components/BoldTableCell";


const EmployeeList = () => {
    const theme = useTheme();
    const context = useContext(EmployeeContext);
    const navigate = useNavigate();

    if (!context) {
        throw new Error('EmployeeList must be used within an EmployeeProvider');
    }

    const { employees } = context;

    const getSortableValue = (employee: Employee, key: string) => {
        switch (key) {
            case 'subdivision.name':
                return employee.subdivision.name;
            case 'position.name':
                return employee.position.name;
            case 'status.name':
                return employee.status.name;
            case 'peoplePartner.fullName':
                return employee.peoplePartner?.fullName || '';
            default:
                if (key in employee) {
                    const value = employee[key as keyof Employee];
                    if (typeof value === "number") return value;
                    return value?.toString();
                }
        }
    };

    const initialSortConfig: SortConfig<Employee> = { key: 'id', direction: 'asc' };
    const { sortedItems: sortedEmployees, sortConfig, handleSort } = useSortableData(employees, initialSortConfig, getSortableValue);

    function handleAddEmployee(): void {
        navigate('/employees/new');
    }

    function handleEditEmployee(id: number): void {
        navigate(`/employees/${id}/edit`);
    }

    function handleViewEmployee(id: number): void {
        navigate(`/employees/${id}`);
    }

    function handleAssignEmployeeToProject(id: number): void {
        navigate(`/employees/${id}/assign-to-project`);
    }

    return (
        <>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2, fontWeight: 'bold', color: theme.palette.primary.main }} variant='h4'>Employees</Typography>
                <Button onClick={() => handleAddEmployee()} sx={{ m: 2 }} size='large' variant='contained'>Add</Button>
            </Box>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <SortableTableCell label="ID" sortKey="id" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Full Name" sortKey="fullName" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Subdivision" sortKey="subdivision.name" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Position" sortKey="position.name" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Status" sortKey="status.name" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="People Partner" sortKey="peoplePartner.fullName" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Out-of-Office Balance" sortKey="outOfOfficeBalance" sortConfig={sortConfig} handleSort={handleSort} />
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {sortedEmployees.map((employee) => (
                            <TableRow key={employee.id}>
                                <TableCell>{employee.id} </TableCell>
                                <BoldTableCell>{employee.fullName}</BoldTableCell>
                                <TableCell>{employee.subdivision.name}</TableCell>
                                <TableCell>{employee.position.name}</TableCell>
                                <TableCell>{employee.status.name}</TableCell>
                                <TableCell>{employee.peoplePartner?.fullName}</TableCell>
                                <TableCell align='center'>{employee.outOfOfficeBalance}</TableCell>
                                <TableCell>
                                    <EditButton id={employee.id} handleEdit={handleEditEmployee} />
                                    <Button size="small" onClick={() => handleAssignEmployeeToProject(employee.id)}>Assign to project</Button>
                                    <ViewButton id={employee.id} handleView={handleViewEmployee} />
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
};

export default EmployeeList;
