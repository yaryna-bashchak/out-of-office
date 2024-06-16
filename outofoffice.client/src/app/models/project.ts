import { Employee } from './employee'

export interface Project {
  id: number
  startDate: string
  endDate: string
  comment: string
  projectManager: Employee
  projectType: ProjectType
  status: ProjectStatus
  members: Employee[]
}

export interface ProjectType {
  id: number
  name: string
}

export interface ProjectStatus {
  id: number
  name: string
}
