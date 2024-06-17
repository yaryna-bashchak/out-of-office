import { useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Box, Typography, Button, useTheme } from '@mui/material';
import LeaveRequestContext from '../../app/context/LeaveRequestContext';
import DetailItem from '../../app/components/DetailItem';

const LeaveRequestDetails = () => {
    const theme = useTheme();
    const context = useContext(LeaveRequestContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!context) {
        throw new Error('LeaveRequestDetails must be used within a LeaveRequestProvider');
    }

    const { leaveRequests } = context;
    const leaveRequest = id ? leaveRequests.find(req => req.id === parseInt(id)) : null;

    const handleGoBack = () => {
        navigate('/leave-requests');
    };

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h3" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                Leave Request Details
            </Typography>
            <DetailItem label='Employee' value={leaveRequest?.employee.fullName} />
            <DetailItem label='Absence Reason' value={leaveRequest?.absenceReason.name} />
            <DetailItem label='Start Date' value={leaveRequest ? new Date(leaveRequest.startDate).toLocaleDateString() : null} />
            <DetailItem label='End Date' value={leaveRequest ? new Date(leaveRequest.endDate).toLocaleDateString() : null} />
            <DetailItem label='Request Type' value={leaveRequest?.requestType.name} />
            <DetailItem label='Hours' value={leaveRequest?.hours} />
            <DetailItem label='Comment' value={leaveRequest?.comment} />
            <DetailItem label='Status' value={leaveRequest?.status.name} />
            <Button variant="outlined" onClick={handleGoBack} >
                Back to Leave Requests List
            </Button>
        </Box>
    );
};

export default LeaveRequestDetails;
