import { createContext, ReactNode, useState, useEffect, useContext } from "react";
import agent from "../api/agent";
import { Project, ProjectStatus, ProjectType, ProjectPayload, ProjectEmployeePayload } from "../models/project";
import EmployeeContext from "./EmployeeContext";

interface ProjectContextType {
    projects: Project[];
    statuses: ProjectStatus[];
    types: ProjectType[];
    addProject: (project: ProjectPayload) => Promise<void>;
    editProject: (id: number, project: ProjectPayload) => Promise<void>;
    addProjectEmployee: (employee: ProjectEmployeePayload) => Promise<void>;
    editProjectEmployee: (employee: ProjectEmployeePayload) => Promise<void>;
}

const ProjectContext = createContext<ProjectContextType | undefined>(undefined);

interface ProjectProviderProps {
    children: ReactNode;
}

export const ProjectProvider = ({ children }: ProjectProviderProps) => {
    const [projects, setProjects] = useState<Project[]>([]);
    const [statuses, setStatuses] = useState<ProjectStatus[]>([]);
    const [types, setTypes] = useState<ProjectType[]>([]);
    const context = useContext(EmployeeContext);

    if (!context) {
        throw new Error('ProjectProvider must be used within an EmployeeProvider');
    }

    const { employees } = context;
    
    useEffect(() => {
        const loadProjects = async () => {
            const projects = await agent.Project.getAll();
            setProjects(projects);
        };
        loadProjects();

        const loadStatuses = async () => {
            const statuses = await agent.Project.getStatuses();
            setStatuses(statuses);
        };
        loadStatuses();

        const loadTypes = async () => {
            const types = await agent.Project.getTypes();
            setTypes(types);
        };
        loadTypes();
    }, []);

    const addProject = async (project: ProjectPayload) => {
        const newProject = await agent.Project.add(project);
        setProjects([...projects, newProject]);
    };

    const editProject = async (id: number, project: ProjectPayload) => {
        const updatedProject = await agent.Project.update(id, project);
        setProjects(projects.map(proj => (proj.id === id ? updatedProject : proj)));
    };

    const addProjectEmployee = async (employee: ProjectEmployeePayload) => {
        await agent.Project.addEmployee(employee);
        const newEmployee = employees.find(emp => emp.id === employee.employeeId);
        if (newEmployee) {
            setProjects(projects.map(proj => 
                proj.id === employee.projectId ? { ...proj, members: [...proj.members, newEmployee] } : proj
            ));
        }
    };

    const editProjectEmployee = async (employee: ProjectEmployeePayload) => {
        await agent.Project.updateEmployee(employee);
        const updatedEmployee = employees.find(emp => emp.id === employee.employeeId);
        if (updatedEmployee) {
            setProjects(projects.map(proj => 
                proj.id === employee.projectId ? { ...proj, members: proj.members.map(mem => 
                    mem.id === employee.employeeId ? updatedEmployee : mem
                )} : proj
            ));
        }
    };

    return (
        <ProjectContext.Provider value={{ projects, statuses, types, addProject, editProject, addProjectEmployee, editProjectEmployee }}>
            {children}
        </ProjectContext.Provider>
    );
};

export default ProjectContext;