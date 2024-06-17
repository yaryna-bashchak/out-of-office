import { useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Box, Typography, Button, useTheme, Paper } from '@mui/material';
import DetailItem from '../../app/components/DetailItem';
import ApprovalRequestContext from '../../app/context/ApprovalRequestContext';

const ApprovalRequestDetails = () => {
    const theme = useTheme();
    const context = useContext(ApprovalRequestContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!context) {
        throw new Error('ApprovalRequestDetails must be used within a ApprovalRequestProvider');
    }

    const { approvalRequests } = context;
    const approvalRequest = id ? approvalRequests.find(req => req.id === parseInt(id)) : null;

    const handleGoBack = () => {
        navigate('/approval-requests');
    };

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h3" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                Approval Request Details
            </Typography>
            <Box component={Paper} sx={{ p: 2, mb: 2 }}>
                <DetailItem label='Employee' value={approvalRequest?.leaveRequest.employee.fullName} />
                <DetailItem label='Absence Reason' value={approvalRequest?.leaveRequest.absenceReason.name} />
                <DetailItem label='Start Date' value={approvalRequest ? new Date(approvalRequest.leaveRequest.startDate).toLocaleDateString() : null} />
                <DetailItem label='End Date' value={approvalRequest ? new Date(approvalRequest.leaveRequest.endDate).toLocaleDateString() : null} />
                <DetailItem label='Request Type' value={approvalRequest?.leaveRequest.requestType.name} />
                <DetailItem label='Hours' value={approvalRequest?.leaveRequest.hours} />
                <DetailItem label='Employee comment' value={approvalRequest?.leaveRequest.comment} styles={{ mb: 0 }} />
            </Box>
            <DetailItem label='Approver' value={approvalRequest?.approver.fullName} />
            <DetailItem label='Status' value={approvalRequest?.status.name} />
            <DetailItem label='Approver comment' value={approvalRequest?.comment} />
            <Button variant="outlined" onClick={handleGoBack} >
                Back to Leave Requests List
            </Button>
        </Box>
    );
};

export default ApprovalRequestDetails;