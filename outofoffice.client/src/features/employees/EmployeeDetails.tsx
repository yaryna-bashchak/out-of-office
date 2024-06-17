import { useContext } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, useTheme } from '@mui/material';
import { useNavigate, useParams } from 'react-router-dom';
import DetailItem from '../../app/components/DetailItem';

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
        <Box sx={{ p: 4 }}>
            <Typography variant="h3" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                {employee?.fullName}
            </Typography>
            <DetailItem label='Subdivision' value={employee?.subdivision.name} />
            <DetailItem label='Position' value={employee?.position.name} />
            <DetailItem label='People Partner' value={employee?.peoplePartner?.fullName} />
            <DetailItem label='Out-Of-Office Balance' value={employee?.outOfOfficeBalance} />
            <DetailItem label='Status' value={employee?.status.name} />
            <Button variant="outlined" onClick={handleGoBack} >
                Back to Employees List
            </Button>
        </Box>
    );
};

export default EmployeeDetails;