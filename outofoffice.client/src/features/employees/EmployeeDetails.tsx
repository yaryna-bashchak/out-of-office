import { useContext } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, useTheme } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';

const EmployeeDetails = () => {
    const theme = useTheme();
    const context = useContext(EmployeeContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!context) {
        throw new Error('EmployeeDetails must be used within an EmployeeProvider');
    }

    const { employees } = context;
    const employee = id ? employees.find(emp => emp.id === parseInt(id)) : null;

    const handleGoBack = () => {
        navigate('/employees');
    };

    return (
        <Box sx={{ p: 4}}>
            <Typography variant="h3" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                {employee?.fullName}
            </Typography>
            <Typography variant="h5" sx={{ mb: 2, color: theme.palette.text.secondary }}>
                Subdivision: <Box component="span" sx={{ fontWeight: 'bold', color: theme.palette.text.primary }}>{employee?.subdivision.name}</Box>
            </Typography>
            <Typography variant="h5" sx={{ mb: 2, color: theme.palette.text.secondary }}>
                Position: <Box component="span" sx={{ fontWeight: 'bold', color: theme.palette.text.primary }}>{employee?.position.name}</Box>
            </Typography>
            <Typography variant="h5" sx={{ mb: 2, color: theme.palette.text.secondary }}>
                People Partner: <Box component="span" sx={{ fontWeight: 'bold', color: theme.palette.text.primary }}>{employee?.peoplePartner?.fullName}</Box>
            </Typography>
            <Typography variant="h5" sx={{ mb: 2, color: theme.palette.text.secondary }}>
                Out-Of-Office Balance: <Box component="span" sx={{ fontWeight: 'bold', color: theme.palette.text.primary }}>{employee?.outOfOfficeBalance} days</Box>
            </Typography>
            <Typography variant="h5" sx={{ mb: 4, color: theme.palette.text.secondary }}>
                Status: <Box component="span" sx={{ fontWeight: 'bold', color: theme.palette.text.primary }}>{employee?.status.name}</Box>
            </Typography>
            <Button variant="outlined" onClick={handleGoBack} >
                Back to Employees List
            </Button>
        </Box>
    );
};

export default EmployeeDetails;