import { Employee } from "./employee"

export interface LeaveRequest {
  id: number
  startDate: string
  endDate: string
  hours: number | null
  comment: string
  absenceReason: AbsenceReason
  employee: Employee
  requestType: RequestType
  status: LeaveRequestStatus
}

export interface AbsenceReason {
  id: number
  name: string
}

export interface RequestType {
  id: number
  name: string
}

export interface LeaveRequestStatus {
  id: number
  name: string
}

export interface LeaveRequestPayload {
  startDate: string
  endDate: string
  hours: number | null
  comment: string
  absenceReasonId: number
  employeeId: number
  requestTypeId: number
}
