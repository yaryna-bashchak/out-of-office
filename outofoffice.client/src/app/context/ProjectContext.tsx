import { createContext, ReactNode, useState, useEffect } from "react";
import agent from "../api/agent";
import { Project, ProjectStatus, ProjectType, ProjectPayload, ProjectEmployee } from "../models/project";

interface ProjectContextType {
    projects: Project[];
    statuses: ProjectStatus[];
    types: ProjectType[];
    addProject: (project: ProjectPayload) => Promise<void>;
    editProject: (id: number, project: ProjectPayload) => Promise<void>;
    addProjectEmployee: (employee: ProjectEmployee) => Promise<void>;
    editProjectEmployee: (employee: ProjectEmployee) => Promise<void>;
}

const ProjectContext = createContext<ProjectContextType | undefined>(undefined);

interface ProjectProviderProps {
    children: ReactNode;
}

export const ProjectProvider = ({ children }: ProjectProviderProps) => {
    const [projects, setProjects] = useState<Project[]>([]);
    const [statuses, setStatuses] = useState<ProjectStatus[]>([]);
    const [types, setTypes] = useState<ProjectType[]>([]);

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

    const addProjectEmployee = async (employee: ProjectEmployee) => {
        await agent.Project.addEmployee(employee);
        setProjects(projects.map(proj =>
            proj.id === employee.projectId ? { ...proj, projectEmployees: [...proj.projectEmployees, employee] } : proj
        ));
    };

    const editProjectEmployee = async (employee: ProjectEmployee) => {
        await agent.Project.updateEmployee(employee);
        setProjects(projects.map(proj =>
            proj.id === employee.projectId ? {
                ...proj, projectEmployees: proj.projectEmployees.map(mem =>
                    mem.employeeId === employee.employeeId ? employee : mem
                )
            } : proj
        ));
    };

    return (
        <ProjectContext.Provider value={{ projects, statuses, types, addProject, editProject, addProjectEmployee, editProjectEmployee }}>
            {children}
        </ProjectContext.Provider>
    );
};

export default ProjectContext;