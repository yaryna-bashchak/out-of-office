import { createContext, useState, useEffect, ReactNode, useContext, Dispatch, SetStateAction } from 'react';
import agent from '../api/agent';
import { AbsenceReason, LeaveRequest, LeaveRequestPayload, LeaveRequestStatus, RequestType } from '../models/leaveRequest';
import ApprovalRequestContext from './ApprovalRequestContext';

interface LeaveRequestContextType {
  leaveRequests: LeaveRequest[];
  statuses: LeaveRequestStatus[];
  types: RequestType[];
  absenceReasons: AbsenceReason[];
  addLeaveRequest: (leaveRequest: LeaveRequestPayload) => Promise<void>;
  editLeaveRequestInfo: (id: number, leaveRequest: LeaveRequestPayload) => Promise<void>;
  editLeaveRequestStatus: (id: number, statusId: number,) => Promise<void>;
  setLeaveRequests: Dispatch<SetStateAction<LeaveRequest[]>>;
  setSearchTerm: Dispatch<SetStateAction<string | undefined>>;
}

const LeaveRequestContext = createContext<LeaveRequestContextType | undefined>(undefined);

interface LeaveRequestProviderProps {
  children: ReactNode;
}

export const LeaveRequestProvider = ({ children }: LeaveRequestProviderProps) => {
  const [leaveRequests, setLeaveRequests] = useState<LeaveRequest[]>([]);
  const [statuses, setStatuses] = useState<LeaveRequestStatus[]>([]);
  const [types, setTypes] = useState<RequestType[]>([]);
  const [absenceReasons, setAbsenceReasons] = useState<AbsenceReason[]>([]);
  const [searchTerm, setSearchTerm] = useState<string | undefined>(undefined);

  const approvalRequestContext = useContext(ApprovalRequestContext);
  
  useEffect(() => {
    const loadLeaveRequests = async () => {
      const leaveRequests = await agent.LeaveRequest.getAll(searchTerm);
      setLeaveRequests(leaveRequests);
    };
    loadLeaveRequests();
  }, [searchTerm]);

  useEffect(() => {
    const loadStatuses = async () => {
      const statuses = await agent.LeaveRequest.getStatuses();
      setStatuses(statuses);
    };
    loadStatuses();

    const loadTypes = async () => {
      const types = await agent.LeaveRequest.getTypes();
      setTypes(types);
    };
    loadTypes();

    const loadAbsenceReasons = async () => {
      const absenceReasons = await agent.LeaveRequest.getAbsenceReasons();
      setAbsenceReasons(absenceReasons);
    };
    loadAbsenceReasons();
  }, []);

  const addLeaveRequest = async (leaveRequest: LeaveRequestPayload) => {
    const newLeaveRequest = await agent.LeaveRequest.add(leaveRequest);
    setLeaveRequests([...leaveRequests, newLeaveRequest]);
  };

  const editLeaveRequestInfo = async (id: number, leaveRequest: LeaveRequestPayload) => {
    const updatedLeaveRequest = await agent.LeaveRequest.updateInfo(id, leaveRequest);
    setLeaveRequests(leaveRequests.map(req => (req.id === id ? updatedLeaveRequest : req)));
  };

  const editLeaveRequestStatus = async (id: number, statusId: number) => {
    const updatedLeaveRequest = await agent.LeaveRequest.updateStatus(id, statusId);
    setLeaveRequests(leaveRequests.map(req => (req.id === id ? updatedLeaveRequest : req)));

    if (approvalRequestContext) {
      const newApprovalRequests = await agent.ApprovalRequest.getAll();
      approvalRequestContext.setApprovalRequests(newApprovalRequests);
    }
  };

  return (
    <LeaveRequestContext.Provider value={{ leaveRequests, types, statuses, absenceReasons, addLeaveRequest, editLeaveRequestInfo, editLeaveRequestStatus, setLeaveRequests, setSearchTerm }}>
      {children}
    </LeaveRequestContext.Provider>
  );
};

export default LeaveRequestContext;

