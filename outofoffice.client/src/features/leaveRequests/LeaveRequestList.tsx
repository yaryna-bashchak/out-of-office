import { useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material';
import { styled, useTheme } from '@mui/material/styles';
import LeaveRequestContext from '../../app/context/LeaveRequestContext';
import { LeaveRequest } from '../../app/models/leaveRequest';
import { CancelButton, EditButton, SubmitButton, ViewButton } from './ListButtons';
import { SortConfig, useSortableData } from '../../app/hooks/useSortableData';
import SortableTableCell from '../../app/components/SortableTableCell';
import { getStatusStyles } from '../../app/components/getStatusStyles';

const BoldTableCell = styled(TableCell)({
    fontWeight: 'bold',
});

const LeaveRequestList = () => {
    const theme = useTheme();
    const context = useContext(LeaveRequestContext);
    const navigate = useNavigate();

    if (!context) {
        throw new Error('LeaveRequestList must be used within a LeaveRequestProvider');
    }

    const { leaveRequests, editLeaveRequestStatus, statuses } = context;

    const getSortableValue = (leaveRequest: LeaveRequest, key: string) => {
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
                if (key in leaveRequest) {
                    const value = leaveRequest[key as keyof LeaveRequest];
                    if (typeof value === "number") return value;
                    return value?.toString();
                }
        }
    };

    const initialSortConfig: SortConfig<LeaveRequest> = { key: 'id', direction: 'asc' };
    const { sortedItems: sortedLeaveRequests, sortConfig, handleSort } = useSortableData(leaveRequests, initialSortConfig, getSortableValue);

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
                            <SortableTableCell label="ID" sortKey="id" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Employee" sortKey="employee.fullName" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Absence Reason" sortKey="absenceReason.name" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Request Type" sortKey="requestType.name" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Start Date" sortKey="startDate" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="End Date" sortKey="endDate" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Hours" sortKey="hours" sortConfig={sortConfig} handleSort={handleSort} />
                            <SortableTableCell label="Status" sortKey="status.name" sortConfig={sortConfig} handleSort={handleSort} />
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
