import { createContext, useState, useEffect, ReactNode } from 'react';
import agent from '../api/agent';
import { ApprovalRequest, ApprovalRequestPayload, ApprovalRequestStatus } from '../models/approvalRequest';

interface ApprovalRequestContextType {
    approvalRequests: ApprovalRequest[];
    statuses: ApprovalRequestStatus[];
    editApprovalRequest: (id: number, approvalRequest: ApprovalRequestPayload) => Promise<void>;
}

const ApprovalRequestContext = createContext<ApprovalRequestContextType | undefined>(undefined);

interface ApprovalRequestProviderProps {
    children: ReactNode;
}

export const ApprovalRequestProvider = ({ children }: ApprovalRequestProviderProps) => {
    const [approvalRequests, setApprovalRequests] = useState<ApprovalRequest[]>([]);
    const [statuses, setStatuses] = useState<ApprovalRequestStatus[]>([]);

    useEffect(() => {
        const loadApprovalRequests = async () => {
            const approvalRequests = await agent.ApprovalRequest.getAll();
            setApprovalRequests(approvalRequests);
        };
        loadApprovalRequests();

        const loadStatuses = async () => {
            const statuses = await agent.ApprovalRequest.getStatuses();
            setStatuses(statuses);
        };
        loadStatuses();
    }, []);

    const editApprovalRequest = async (id: number, approvalRequest: ApprovalRequestPayload) => {
        const updatedApprovalRequest = await agent.ApprovalRequest.update(id, approvalRequest);
        setApprovalRequests(approvalRequests.map(req => (req.id === id ? updatedApprovalRequest : req)));
    };

    return (
        <ApprovalRequestContext.Provider value={{ approvalRequests, statuses, editApprovalRequest }}>
            {children}
        </ApprovalRequestContext.Provider>
    );
};

export default ApprovalRequestContext;