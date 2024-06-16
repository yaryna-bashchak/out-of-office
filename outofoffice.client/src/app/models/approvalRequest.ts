import { Employee } from './employee'
import { LeaveRequest } from './leaveRequest'

export interface ApprovalRequest {
  id: number
  comment: string
  approver: Employee
  leaveRequest: LeaveRequest
  status: ApprovalRequestStatus
}

export interface ApprovalRequestStatus {
  id: number
  name: string
}
