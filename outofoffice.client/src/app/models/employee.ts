export interface Employee {
  id: number
  fullName: string
  outOfOfficeBalance: number
  photo: string | null
  peoplePartner: PeoplePartner | null
  position: Position
  status: EmployeeStatus
  subdivision: Subdivision
}

export interface PeoplePartner {
  id: number
  fullName: string
}

export interface Position {
  id: number
  name: string
}

export interface EmployeeStatus {
  id: number
  name: string
}

export interface Subdivision {
  id: number
  name: string
}

export interface EmployeePayload {
  fullName: string
  outOfOfficeBalance: number
  positionId: number
  statusId: number
  subdivisionId: number
  photo: string | null
  peoplePartnerId: number | null
}
