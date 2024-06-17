import React from 'react';
import { Button } from '@mui/material';
import { LeaveRequest } from '../../app/models/leaveRequest';

export const CancelButton: React.FC<{ leaveRequest: LeaveRequest; handleCancelLeaveRequest: (id: number) => void }> = ({
    leaveRequest,
    handleCancelLeaveRequest,
}) => (
    <Button
        size="small"
        variant="outlined"
        color="error"
        onClick={() => handleCancelLeaveRequest(leaveRequest.id)}
        disabled={leaveRequest.status.name !== 'New' && leaveRequest.status.name !== 'Submitted'}
        sx={{ width: '70px' }}
    >
        {leaveRequest.status.name === 'Cancelled' ? 'Cancelled' : 'Cancel'}
    </Button>
);

export const ViewButton: React.FC<{ leaveRequest: LeaveRequest; handleViewLeaveRequest: (id: number) => void }> = ({
    leaveRequest,
    handleViewLeaveRequest,
}) => (
    <Button size="small" onClick={() => handleViewLeaveRequest(leaveRequest.id)}>
        View
    </Button>
);

export const EditButton: React.FC<{ leaveRequest: LeaveRequest; handleEditLeaveRequest: (id: number) => void }> = ({
    leaveRequest,
    handleEditLeaveRequest,
}) => (
    <Button
        size="small"
        onClick={() => handleEditLeaveRequest(leaveRequest.id)}
        disabled={leaveRequest.status.name !== 'New'}
    >
        Edit
    </Button>
);

export const SubmitButton: React.FC<{ leaveRequest: LeaveRequest; handleSubmitLeaveRequest: (id: number) => void }> = ({
    leaveRequest,
    handleSubmitLeaveRequest,
}) => (
    <Button
        size="small"
        variant="contained"
        color="success"
        onClick={() => handleSubmitLeaveRequest(leaveRequest.id)}
        disabled={leaveRequest.status.name !== 'New'}
        sx={{ width: '70px' }}
    >
        {leaveRequest.status.name === 'New' ? 'Submit' : 'Submitted'}
    </Button>
);
