import axios, { AxiosResponse } from "axios"

axios.defaults.baseURL = import.meta.env.VITE_API_URL

const responseBody = (response: AxiosResponse) => response.data

const requests = {
    get: (url: string, params?: URLSearchParams) =>
        axios.get(url, { params }).then(responseBody),
    post: (url: string, body: object) => axios.post(url, body).then(responseBody),
    put: (url: string, body: object) => axios.put(url, body).then(responseBody),
    putWithoutBody: (url: string) => axios.put(url).then(responseBody),
}

const Employee = {
    getAll: () => requests.get('employees'),
    getById: (id: number) => requests.get(`employees/${id}`),
    add: (body: object) => requests.post('employees', body),
    update: (id: number, body: object) => requests.put(`employees/${id}`, body),
    getSubdivisions: () => requests.get('employees/subdivisions'),
    getPositions: () => requests.get('employees/positions'),
    getStatuses: () => requests.get('employees/statuses'),
    getHRManagers: () => requests.get('employees/hr-managers'),
    getProjectManagers: () => requests.get('employees/project-managers'),
}

const LeaveRequest = {
    getAll: () => requests.get('leaveRequests'),
    getById: (id: number) => requests.get(`leaveRequests/${id}`),
    add: (body: object) => requests.post('leaveRequests', body),
    updateInfo: (id: number, body: object) => requests.put(`leaveRequests/${id}`, body),
    updateStatus: (id: number, statusId: number) => requests.putWithoutBody(`leaveRequests/${id}/${statusId}`),
    getAbsenceReasons: () => requests.get('leaveRequests/absenceReasons'),
    getStatuses: () => requests.get('leaveRequests/statuses'),
    getTypes: () => requests.get('leaveRequests/types'),
};

const ApprovalRequest = {
    getAll: () => requests.get('approvalRequests'),
    getById: (id: number) => requests.get(`approvalRequests/${id}`),
    update: (id: number, body: object) => requests.put(`approvalRequests/${id}`, body),
    getStatuses: () => requests.get('approvalRequests/statuses'),
};

const Project = {
    getAll: () => requests.get('projects'),
    getById: (id: number) => requests.get(`projects/${id}`),
    add: (body: object) => requests.post('projects', body),
    update: (id: number, body: object) => requests.put(`projects/${id}`, body),
    addEmployee: (body: object) => requests.post('projects/addEmployee', body),
    updateEmployee: (body: object) => requests.put('projects/updateEmployee', body),
    getStatuses: () => requests.get('projects/statuses'),
    getTypes: () => requests.get('projects/types'),
};

const agent = {
    Employee,
    LeaveRequest,
    ApprovalRequest,
    Project,
};

export default agent