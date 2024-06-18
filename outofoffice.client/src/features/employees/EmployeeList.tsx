import { useContext, useEffect } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { Employee } from '../../app/models/employee';
import { SortConfig, useSortableData } from '../../app/hooks/useSortableData';
import SortableTableCell from '../../app/components/SortableTableCell';
import { EditButton, ViewButton } from '../../app/components/ListButtons';
import BoldTableCell from "../../app/components/BoldTableCell";
import UserRoleContext from '../../app/context/UserRoleContext';
import SearchLine from '../../app/components/SearchLine';

const EmployeeList = () => {
    const theme = useTheme();
    const context = useContext(EmployeeContext);
    const userRoleContext = useContext(UserRoleContext);
    const navigate = useNavigate();

    if (!context || !userRoleContext) {
        throw new Error('EmployeeList must be used within an EmployeeProvider and UserRoleProvider');
    }

    const { filteredEmployees, setSearchTerm } = context;
    const { userRole } = userRoleContext;
    
    useEffect(() => {
        setSearchTerm(undefined);
    }, [setSearchTerm]);

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
    const { sortedItems: sortedEmployees, sortConfig, handleSort } = useSortableData(filteredEmployees, initialSortConfig, getSortableValue);

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
    
    const handleSearch = (term: string) => {
        setSearchTerm(term);
    };
    
    return (
        <Box sx={{ minWidth: '800px' }}>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2, fontWeight: 'bold', color: theme.palette.primary.main }} variant='h4'>Employees</Typography>
                {userRole === 'HR Manager' && <Button onClick={() => handleAddEmployee()} sx={{ m: 2 }} size='large' variant='contained'>Add</Button>}
            </Box>
            <SearchLine handleSearch={handleSearch} label='full name' styles={{ mb: '16px' }} />
            {sortedEmployees.length === 0 ?
                <Typography>There is no items.</Typography> :
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
                                        {userRole === 'HR Manager' && <EditButton id={employee.id} handleEdit={handleEditEmployee} />}
                                        {userRole === 'Project Manager' && <Button size="small" onClick={() => handleAssignEmployeeToProject(employee.id)}>Assign to project</Button>}
                                        <ViewButton id={employee.id} handleView={handleViewEmployee} />
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>}
        </Box>
    );
};

export default EmployeeList;
