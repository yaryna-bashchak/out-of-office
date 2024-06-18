import { useContext } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, useTheme, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import DetailItem from '../../app/components/DetailItem';
import BoldTableCell from '../../app/components/BoldTableCell';
import ProjectContext from '../../app/context/ProjectContext';

const EmployeeDetails = () => {
    const theme = useTheme();
    const projectContext = useContext(ProjectContext);
    const employeeContext = useContext(EmployeeContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!projectContext || !employeeContext) {
        throw new Error('ProjectDetails must be used within a ProjectProvider and EmployeeProvider');
    }

    const { projects } = projectContext;
    const { employees } = employeeContext;
    const employee = id ? employees.find(emp => emp.id === parseInt(id)) : null;

    const handleGoBack = () => {
        navigate('/employees');
    };

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h3" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                {employee?.fullName}
            </Typography>
            <DetailItem label='Subdivision' value={employee?.subdivision.name} />
            <DetailItem label='Position' value={employee?.position.name} />
            <DetailItem label='People Partner' value={employee?.peoplePartner?.fullName} />
            <DetailItem label='Out-Of-Office Balance' value={employee?.outOfOfficeBalance} />
            <DetailItem label='Status' value={employee?.status.name} />
            {employee && employee.projectEmployees.length > 0 && <TableContainer component={Paper} sx={{ mb: 2 }}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Project ID</TableCell>
                            <TableCell>Project Manager</TableCell>
                            <TableCell>Project Type</TableCell>
                            <TableCell>Project Comment</TableCell>
                            <TableCell>Start Date</TableCell>
                            <TableCell>End Date</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {employee?.projectEmployees.map((projectEmployee) => {
                            const project = projects.find(proj => proj.id === projectEmployee.projectId);
                            return (
                                <TableRow key={projectEmployee.projectId}>
                                    <TableCell>{projectEmployee.projectId}</TableCell>
                                    <BoldTableCell>{project && project.projectManager.fullName}</BoldTableCell>
                                    <BoldTableCell>{project && project.projectType.name}</BoldTableCell>
                                    <BoldTableCell>{project && project.comment}</BoldTableCell>
                                    <TableCell>{new Date(projectEmployee.startDate).toLocaleDateString()}</TableCell>
                                    <TableCell>{projectEmployee.endDate && new Date(projectEmployee.endDate).toLocaleDateString()}</TableCell>
                                </TableRow>
                            )
                        })}
                    </TableBody>
                </Table>
            </TableContainer>}
            <Button variant="outlined" onClick={handleGoBack} >
                Back to Employees List
            </Button>
        </Box>
    );
};

export default EmployeeDetails;