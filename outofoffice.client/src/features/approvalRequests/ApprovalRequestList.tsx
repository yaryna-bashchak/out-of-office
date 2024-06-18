import { TableCell, Box, Typography, TableContainer, Paper, Table, TableHead, TableRow, TableBody, useTheme } from "@mui/material";
import { useContext, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import SortableTableCell from "../../app/components/SortableTableCell";
import { getStatusStyles } from "../../app/components/getStatusStyles";
import ApprovalRequestContext from "../../app/context/ApprovalRequestContext";
import { SortConfig, useSortableData } from "../../app/hooks/useSortableData";
import { ApprovalRequest, ApprovalRequestPayload } from "../../app/models/approvalRequest";
import { ApproveButton, RejectButton, ViewButton } from "../../app/components/ListButtons";
import BoldTableCell from "../../app/components/BoldTableCell";
import SearchLine from '../../app/components/SearchLine';

const ApprovalRequestList = () => {
    const theme = useTheme();
    const context = useContext(ApprovalRequestContext);
    const navigate = useNavigate();

    if (!context) {
        throw new Error('ApprovalRequestList must be used within an ApprovalRequestProvider');
    }

    const { filteredApprovalRequests, statuses, editApprovalRequest, setSearchTerm } = context;

    useEffect(() => {
        setSearchTerm(undefined);
    }, [setSearchTerm]);
    
    const getSortableValue = (approvalRequest: ApprovalRequest, key: string) => {
        switch (key) {
            case 'approver.fullName':
                return approvalRequest.approver.fullName;
            case 'leaveRequest.employee.fullName':
                return approvalRequest.leaveRequest.employee.fullName;
            case 'status.name':
                return approvalRequest.status.name;
            default:
                if (key in approvalRequest) {
                    const value = approvalRequest[key as keyof ApprovalRequest];
                    if (typeof value === "number") return value;
                    return value?.toString();
                }
        }
    };

    const initialSortConfig: SortConfig<ApprovalRequest> = { key: 'id', direction: 'asc' };
    const { sortedItems: sortedApprovalRequests, sortConfig, handleSort } = useSortableData(filteredApprovalRequests, initialSortConfig, getSortableValue);

    function handleViewApprovalRequest(id: number): void {
        navigate(`/approval-requests/${id}`);
    }

    function handleApproveRequest(id: number): void {
        const status = statuses.find(s => s.name === "Approved");
        if (status) {
            const transformedData: ApprovalRequestPayload = {
                statusId: status.id,
                comment: null,
            };
            editApprovalRequest(id, transformedData);
        }
    }

    function handleRejectRequest(id: number): void {
        navigate(`/approval-requests/${id}/reject`);
    }

    const handleSearch = (term: string) => {
        setSearchTerm(term);
    };

    return (
        <Box sx={{ minWidth: '800px' }}>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2, fontWeight: 'bold', color: theme.palette.primary.main }} variant='h4'>Approval Requests</Typography>
            </Box>
            <SearchLine handleSearch={handleSearch} label='id' styles={{ mb: '16px' }} />
            {sortedApprovalRequests.length === 0 ?
                <Typography>There is no items.</Typography> :
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <SortableTableCell label="ID" sortKey="id" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="Approver" sortKey="approver.fullName" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="Employee" sortKey="leaveRequest.employee.fullName" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="Leave Request Type" sortKey="leaveRequest.requestType.name" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="Status" sortKey="status.name" sortConfig={sortConfig} handleSort={handleSort} />
                                <TableCell></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {sortedApprovalRequests.map((approvalRequest) => (
                                <TableRow key={approvalRequest.id}>
                                    <TableCell>{approvalRequest.id}</TableCell>
                                    <BoldTableCell>{approvalRequest.approver.fullName}</BoldTableCell>
                                    <TableCell>{approvalRequest.leaveRequest.employee.fullName}</TableCell>
                                    <TableCell>{approvalRequest.leaveRequest.requestType.name}</TableCell>
                                    <TableCell><Typography align='center' sx={getStatusStyles(approvalRequest.status.name)}>{approvalRequest.status.name}</Typography></TableCell>
                                    <TableCell>
                                        <RejectButton id={approvalRequest.id} statusName={approvalRequest.status.name} handleReject={handleRejectRequest} />
                                        <ViewButton id={approvalRequest.id} handleView={handleViewApprovalRequest} />
                                        <ApproveButton id={approvalRequest.id} statusName={approvalRequest.status.name} handleApprove={handleApproveRequest} />
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer >}
        </Box>
    );
};

export default ApprovalRequestList;