import { Employee } from './employee'
import { LeaveRequest } from './leaveRequest'

export interface ApprovalRequest {
  id: number
  comment: string | null
  approver: Employee
  leaveRequest: LeaveRequest
  status: ApprovalRequestStatus
}

export interface ApprovalRequestStatus {
  id: number
  name: string
}

export interface ApprovalRequestPayload {
  comment: string | null
  statusId: number
}