import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import { RouterProvider } from 'react-router-dom'
import { router } from './app/router/Routes'
import { EmployeeProvider } from './app/context/EmployeeContext'
import { LeaveRequestProvider } from './app/context/LeaveRequestContext'
import { ApprovalRequestProvider } from './app/context/ApprovalRequestContext'
import { ProjectProvider } from './app/context/ProjectContext'
import { UserRoleProvider } from './app/context/UserRoleContext'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <UserRoleProvider>
      <EmployeeProvider>
        <LeaveRequestProvider>
          <ApprovalRequestProvider>
            <ProjectProvider>
              <RouterProvider router={router} />
            </ProjectProvider>
          </ApprovalRequestProvider>
        </LeaveRequestProvider>
      </EmployeeProvider>
    </UserRoleProvider>
  </React.StrictMode>,
)
