import { useContext, useState } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, styled, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { Employee } from '../../app/models/employee';
import { SouthEast, NorthEast } from '@mui/icons-material';

const BoldTableCell = styled(TableCell)({
    fontWeight: 'bold',
});

const EmployeeList = () => {
    const theme = useTheme();
    const context = useContext(EmployeeContext);
    const navigate = useNavigate();

    if (!context) {
        throw new Error('EmployeeList must be used within an EmployeeProvider');
    }

    const { employees } = context;

    const [sortConfig, setSortConfig] = useState<{ key: keyof Employee | 'subdivision.name' | 'position.name' | 'status.name' | 'peoplePartner.fullName'; direction: 'asc' | 'desc' }>({ key: 'id', direction: 'asc' });

    const getSortableValue = (employee: Employee, key: keyof Employee | 'subdivision.name' | 'position.name' | 'status.name' | 'peoplePartner.fullName') => {
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
                return employee[key];
        }
    };

    const sortedEmployees = [...employees].sort((a, b) => {
        const aValue = getSortableValue(a, sortConfig.key);
        const bValue = getSortableValue(b, sortConfig.key);

        if (aValue == null && bValue == null) return 0;
        if (aValue == null) return sortConfig.direction === 'asc' ? -1 : 1;
        if (bValue == null) return sortConfig.direction === 'asc' ? 1 : -1;

        if (typeof aValue === 'string' && typeof bValue === 'string') {
            return sortConfig.direction === 'asc' ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue);
        }

        if (aValue < bValue) {
            return sortConfig.direction === 'asc' ? -1 : 1;
        }
        if (aValue > bValue) {
            return sortConfig.direction === 'asc' ? 1 : -1;
        }
        return 0;
    });

    const handleSort = (key: keyof Employee | 'subdivision.name' | 'position.name' | 'status.name' | 'peoplePartner.fullName') => {
        let direction: 'asc' | 'desc' = 'asc';
        if (sortConfig.key === key && sortConfig.direction === 'asc') {
            direction = 'desc';
        }
        setSortConfig({ key, direction });
    };

    function handleAddEmployee(): void {
        navigate('/employees/new');
    }

    function handleEditEmployee(id: number): void {
        navigate(`/employees/${id}/edit`);
    }

    function handleViewEmployee(id: number): void {
        navigate(`/employees/${id}`);
    }

    const renderSortIndicator = (key: keyof Employee | 'subdivision.name' | 'position.name' | 'status.name' | 'peoplePartner.fullName') => {
        if (sortConfig.key === key) {
            return sortConfig.direction === 'asc' ? <NorthEast fontSize="small" /> : <SouthEast fontSize="small" />;
        }
        return null;
    };

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
                            <TableCell onClick={() => handleSort('id')}>
                                <Box display="flex" alignItems="center">
                                    ID {renderSortIndicator('id')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('fullName')}>
                                <Box display="flex" alignItems="center">
                                    Full Name {renderSortIndicator('fullName')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('subdivision.name')}>
                                <Box display="flex" alignItems="center">
                                    Subdivision {renderSortIndicator('subdivision.name')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('position.name')}>
                                <Box display="flex" alignItems="center">
                                    Position {renderSortIndicator('position.name')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('status.name')}>
                                <Box display="flex" alignItems="center">
                                    Status {renderSortIndicator('status.name')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('peoplePartner.fullName')}>
                                <Box display="flex" alignItems="center">
                                    People Partner {renderSortIndicator('peoplePartner.fullName')}
                                </Box>
                            </TableCell>
                            <TableCell align='center' onClick={() => handleSort('outOfOfficeBalance')}>
                                <Box display="flex" alignItems="center">
                                    Out-of-Office Balance {renderSortIndicator('outOfOfficeBalance')}
                                </Box>
                            </TableCell>
                            <TableCell></TableCell>
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
                                    <Button onClick={() => handleEditEmployee(employee.id)}>Edit</Button>
                                </TableCell>
                                <TableCell>
                                    <Button onClick={() => handleViewEmployee(employee.id)}>View</Button>
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
