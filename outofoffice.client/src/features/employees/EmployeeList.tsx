import { useContext } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, styled, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';

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

    function handleAddEmployee(): void {
        navigate('/employees/new');
    }

    function handleEditEmployee(id: number): void {
        navigate(`/employees/${id}/edit`);
    }

    function handleViewEmployee(id: number): void {
        navigate(`/employees/${id}`);
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
                            <TableCell>ID</TableCell>
                            <TableCell>Full Name</TableCell>
                            <TableCell>Subdivision</TableCell>
                            <TableCell>Position</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>People Partner</TableCell>
                            <TableCell align='center'>Out-of-Office Balance</TableCell>
                            <TableCell></TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {employees.map((employee) => (
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
