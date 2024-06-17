import { Employee } from './employee'

export interface Project {
  id: number
  startDate: string
  endDate: string | null
  comment: string
  projectManager: Employee
  projectType: ProjectType
  status: ProjectStatus
  projectEmployees: ProjectEmployee[]
}

export interface ProjectType {
  id: number
  name: string
}

export interface ProjectStatus {
  id: number
  name: string
}

export interface ProjectPayload {
  startDate: string
  endDate: string | null
  comment: string
  projectManagerId: number
  projectTypeId: number
  statusId: number
}

export interface ProjectEmployee {
  startDate: string
  endDate: string | null
  employeeId: number
  projectId: number
}
