import { useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import LeaveRequestContext from '../../app/context/LeaveRequestContext';
import { LeaveRequest } from '../../app/models/leaveRequest';
import { CancelButton, EditButton, SubmitButton, ViewButton } from '../../app/components/ListButtons';
import { SortConfig, useSortableData } from '../../app/hooks/useSortableData';
import SortableTableCell from '../../app/components/SortableTableCell';
import { getStatusStyles } from '../../app/components/getStatusStyles';
import BoldTableCell from "../../app/components/BoldTableCell";
import UserRoleContext from '../../app/context/UserRoleContext';
import SearchLine from '../../app/components/SearchLine';

const LeaveRequestList = () => {
    const theme = useTheme();
    const context = useContext(LeaveRequestContext);
    const userRoleContext = useContext(UserRoleContext);
    const navigate = useNavigate();

    if (!context || !userRoleContext) {
        throw new Error('LeaveRequestList must be used within a LeaveRequestProvider and UserRoleProvider');
    }

    const { filteredLeaveRequests, editLeaveRequestStatus, statuses, setSearchTerm } = context;
    const { userRole } = userRoleContext;

    useEffect(() => {
        setSearchTerm(undefined);
    }, [setSearchTerm]);

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
    const { sortedItems: sortedLeaveRequests, sortConfig, handleSort } = useSortableData(filteredLeaveRequests, initialSortConfig, getSortableValue);

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

    const handleSearch = (term: string) => {
        setSearchTerm(term);
    };

    return (
        <Box sx={{ minWidth: '800px' }}>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2, fontWeight: 'bold', color: theme.palette.primary.main }} variant='h4'>Leave Requests</Typography>
                {userRole === 'Employee' && <Button onClick={() => handleAddLeaveRequest()} sx={{ m: 2 }} size='large' variant='contained'>Add</Button>}
            </Box>
            <SearchLine handleSearch={handleSearch} label='id' styles={{ mb: '16px' }} />
            {sortedLeaveRequests.length === 0 ?
                <Typography>There is no items.</Typography> :
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
                                        {userRole === 'Employee' && <CancelButton id={leaveRequest.id} statusName={leaveRequest.status.name} handleCancel={handleCancelLeaveRequest} />}
                                        <ViewButton id={leaveRequest.id} handleView={handleViewLeaveRequest} />
                                        {userRole === 'Employee' && <>
                                            <EditButton id={leaveRequest.id} statusName={leaveRequest.status.name} handleEdit={handleEditLeaveRequest} />
                                            <SubmitButton id={leaveRequest.id} statusName={leaveRequest.status.name} handleSubmit={handleSubmitLeaveRequest} />
                                        </>}
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer >}
        </Box>
    );
};

export default LeaveRequestList;
