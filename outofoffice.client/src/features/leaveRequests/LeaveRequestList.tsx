import { useContext, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material';
import { styled, useTheme } from '@mui/material/styles';
import { NorthEast, SouthEast } from '@mui/icons-material';
import LeaveRequestContext from '../../app/context/LeaveRequestContext';
import { LeaveRequest } from '../../app/models/leaveRequest';

const BoldTableCell = styled(TableCell)({
    fontWeight: 'bold',
});

const getStatusStyles = (status: string) => {
    switch (status) {
        case 'New':
            return {
                border: '1px solid blue',
                color: 'blue',
                borderRadius: 1,
                paddingLeft: 0.5,
                paddingRight: 0.5,
            };
        case 'Submitted':
            return {
                border: '1px solid orange',
                color: 'orange',
                borderRadius: 1,
                paddingLeft: 0.5,
                paddingRight: 0.5,
            };
        case 'Cancelled':
            return {
                border: '1px solid gray',
                color: 'gray',
                borderRadius: 1,
                paddingLeft: 0.5,
                paddingRight: 0.5,
            };
        case 'Approved':
            return {
                border: '1px solid green',
                color: 'green',
                borderRadius: 1,
                paddingLeft: 0.5,
                paddingRight: 0.5,
            }
        case 'Rejected':
            return {
                border: '1px solid red',
                color: 'red',
                borderRadius: 1,
                paddingLeft: 0.5,
                paddingRight: 0.5,
            };
        default:
            return {};
    }
};

const CancelButton: React.FC<{ leaveRequest: LeaveRequest; handleCancelLeaveRequest: (id: number) => void }> = ({
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

const ViewButton: React.FC<{ leaveRequest: LeaveRequest; handleViewLeaveRequest: (id: number) => void }> = ({
    leaveRequest,
    handleViewLeaveRequest,
}) => (
    <Button size="small" onClick={() => handleViewLeaveRequest(leaveRequest.id)}>
        View
    </Button>
);

const EditButton: React.FC<{ leaveRequest: LeaveRequest; handleEditLeaveRequest: (id: number) => void }> = ({
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

const SubmitButton: React.FC<{ leaveRequest: LeaveRequest; handleSubmitLeaveRequest: (id: number) => void }> = ({
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

const LeaveRequestList = () => {
    const theme = useTheme();
    const context = useContext(LeaveRequestContext);
    const navigate = useNavigate();

    if (!context) {
        throw new Error('LeaveRequestList must be used within a LeaveRequestProvider');
    }

    const { leaveRequests, editLeaveRequestStatus, statuses } = context;

    const [sortConfig, setSortConfig] = useState<{ key: keyof LeaveRequest | 'employee.fullName' | 'absenceReason.name' | 'requestType.name' | 'status.name'; direction: 'asc' | 'desc' }>({ key: 'id', direction: 'asc' });

    const getSortableValue = (leaveRequest: LeaveRequest, key: keyof LeaveRequest | 'employee.fullName' | 'absenceReason.name' | 'requestType.name' | 'status.name') => {
        switch (key) {
            case 'employee.fullName':
                return leaveRequest.employee.fullName;
            case 'absenceReason.name':
                return leaveRequest.absenceReason.name;
            case 'requestType.name':
                return leaveRequest.requestType.name;
            case 'status.name':
                return leaveRequest.status.name;
            default:
                return leaveRequest[key];
        }
    };

    const sortedLeaveRequests = [...leaveRequests].sort((a, b) => {
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

    const handleSort = (key: keyof LeaveRequest | 'employee.fullName' | 'absenceReason.name' | 'requestType.name' | 'status.name') => {
        let direction: 'asc' | 'desc' = 'asc';
        if (sortConfig.key === key && sortConfig.direction === 'asc') {
            direction = 'desc';
        }
        setSortConfig({ key, direction });
    };

    function handleAddLeaveRequest(): void {
        navigate('/leave-requests/new');
    }

    function handleEditLeaveRequest(id: number): void {
        navigate(`/leave-requests/${id}/edit`);
    }

    function handleViewLeaveRequest(id: number): void {
        navigate(`/leave-requests/${id}`);
    }

    function handleSubmitLeaveRequest(id: number): void {
        const status = statuses.find(s => s.name === "Submitted");
        if (status) {
            editLeaveRequestStatus(id, status.id);
        }
    }

    function handleCancelLeaveRequest(id: number): void {
        const status = statuses.find(s => s.name === "Cancelled");
        if (status) {
            editLeaveRequestStatus(id, status.id);
        }
    }

    const renderSortIndicator = (key: keyof LeaveRequest | 'employee.fullName' | 'absenceReason.name' | 'requestType.name' | 'status.name') => {
        if (sortConfig.key === key) {
            return sortConfig.direction === 'asc' ? <NorthEast fontSize="small" /> : <SouthEast fontSize="small" />;
        }
        return null;
    };

    return (
        <>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2, fontWeight: 'bold', color: theme.palette.primary.main }} variant='h4'>Leave Requests</Typography>
                <Button onClick={() => handleAddLeaveRequest()} sx={{ m: 2 }} size='large' variant='contained'>Add</Button>
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
                            <TableCell onClick={() => handleSort('employee.fullName')}>
                                <Box display="flex" alignItems="center">
                                    Employee {renderSortIndicator('employee.fullName')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('absenceReason.name')}>
                                <Box display="flex" alignItems="center">
                                    Absence Reason {renderSortIndicator('absenceReason.name')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('requestType.name')}>
                                <Box display="flex" alignItems="center">
                                    Request Type {renderSortIndicator('requestType.name')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('startDate')}>
                                <Box display="flex" alignItems="center">
                                    Start Date {renderSortIndicator('startDate')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('endDate')}>
                                <Box display="flex" alignItems="center">
                                    End Date {renderSortIndicator('endDate')}
                                </Box>
                            </TableCell>
                            <TableCell align='center' onClick={() => handleSort('hours')}>
                                <Box display="flex" alignItems="center">
                                    Hours {renderSortIndicator('hours')}
                                </Box>
                            </TableCell>
                            <TableCell onClick={() => handleSort('status.name')}>
                                <Box display="flex" alignItems="center">
                                    Status {renderSortIndicator('status.name')}
                                </Box>
                            </TableCell>
                            <TableCell></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {sortedLeaveRequests.map((leaveRequest) => (
                            <TableRow key={leaveRequest.id}>
                                <TableCell>{leaveRequest.id}</TableCell>
                                <BoldTableCell>{leaveRequest.employee.fullName}</BoldTableCell>
                                <TableCell>{leaveRequest.absenceReason.name}</TableCell>
                                <TableCell>{leaveRequest.requestType.name}</TableCell>
                                <TableCell>{new Date(leaveRequest.startDate).toLocaleDateString()}</TableCell>
                                <TableCell>{new Date(leaveRequest.endDate).toLocaleDateString()}</TableCell>
                                <TableCell align='center'>{leaveRequest.hours}</TableCell>
                                <TableCell><Typography align='center' sx={getStatusStyles(leaveRequest.status.name)}>{leaveRequest.status.name}</Typography></TableCell>
                                <TableCell>
                                    <CancelButton leaveRequest={leaveRequest} handleCancelLeaveRequest={handleCancelLeaveRequest} />
                                    <ViewButton leaveRequest={leaveRequest} handleViewLeaveRequest={handleViewLeaveRequest} />
                                    <EditButton leaveRequest={leaveRequest} handleEditLeaveRequest={handleEditLeaveRequest} />
                                    <SubmitButton leaveRequest={leaveRequest} handleSubmitLeaveRequest={handleSubmitLeaveRequest} />
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer >
        </>
    );
};

export default LeaveRequestList;
