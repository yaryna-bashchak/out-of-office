import React from 'react';
import { Button } from '@mui/material';

export const CancelButton: React.FC<{ id: number; statusName: string; handleCancel: (id: number) => void }> = ({
    id,
    statusName,
    handleCancel
}) => (
    <Button
        size="small"
        variant="outlined"
        color="error"
        onClick={() => handleCancel(id)}
        disabled={statusName !== 'New' && statusName !== 'Submitted'}
        sx={{ width: '70px' }}
    >
        {statusName === 'Cancelled' ? 'Cancelled' : 'Cancel'}
    </Button>
);

export const ViewButton: React.FC<{ id: number; handleView: (id: number) => void }> = ({ id, handleView }) => (
    <Button size="small" onClick={() => handleView(id)}>
        View
    </Button>
);

export const EditButton: React.FC<{ id: number; statusName?: string; handleEdit: (id: number) => void }> = ({
    id,
    statusName,
    handleEdit
}) => (
    <Button
        size="small"
        onClick={() => handleEdit(id)}
        disabled={statusName !== undefined && statusName !== 'New'}
    >
        Edit
    </Button>
);

export const SubmitButton: React.FC<{ id: number; statusName: string; handleSubmit: (id: number) => void }> = ({
    id, statusName,
    handleSubmit,
}) => (
    <Button
        size="small"
        variant="contained"
        color="success"
        onClick={() => handleSubmit(id)}
        disabled={statusName !== 'New'}
        sx={{ width: '70px' }}
    >
        {statusName === 'New' || statusName === 'Cancelled' ? 'Submit' : 'Submitted'}
    </Button>
);

export const ApproveButton: React.FC<{ id: number; statusName: string; handleApprove: (id: number) => void }> = ({
    id, statusName,
    handleApprove,
}) => (
    <Button
        size="small"
        variant="contained"
        color="success"
        onClick={() => handleApprove(id)}
        disabled={statusName !== 'New'}
        sx={{ width: '70px' }}
    >
        {statusName === 'New' || statusName === 'Cancelled' ? 'Approve' : 'Approved'}
    </Button>
);

export const RejectButton: React.FC<{ id: number; statusName: string; handleReject: (id: number) => void }> = ({
    id,
    statusName,
    handleReject
}) => (
    <Button
        size="small"
        variant="outlined"
        color="error"
        onClick={() => handleReject(id)}
        disabled={statusName !== 'New'}
        sx={{ width: '70px' }}
    >
        {statusName === 'Rejected' ? 'Rejected' : 'Reject'}
    </Button>
);