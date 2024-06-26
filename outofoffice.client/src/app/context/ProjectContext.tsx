import { createContext, ReactNode, useState, useEffect, useContext, Dispatch, SetStateAction } from "react";
import agent from "../api/agent";
import { Project, ProjectStatus, ProjectType, ProjectPayload, ProjectEmployee } from "../models/project";
import EmployeeContext from "./EmployeeContext";

interface ProjectContextType {
    projects: Project[];
    filteredProjects: Project[];
    statuses: ProjectStatus[];
    types: ProjectType[];
    addProject: (project: ProjectPayload) => Promise<void>;
    editProject: (id: number, project: ProjectPayload) => Promise<void>;
    addProjectEmployee: (employee: ProjectEmployee) => Promise<void>;
    editProjectEmployee: (employee: ProjectEmployee) => Promise<void>;
    setSearchTerm: Dispatch<SetStateAction<string | undefined>>;
}

const ProjectContext = createContext<ProjectContextType | undefined>(undefined);

interface ProjectProviderProps {
    children: ReactNode;
}

export const ProjectProvider = ({ children }: ProjectProviderProps) => {
    const [projects, setProjects] = useState<Project[]>([]);
    const [filteredProjects, setFilteredProjects] = useState<Project[]>([]);
    const [statuses, setStatuses] = useState<ProjectStatus[]>([]);
    const [types, setTypes] = useState<ProjectType[]>([]);
    const [searchTerm, setSearchTerm] = useState<string | undefined>(undefined);

    const employeeContext = useContext(EmployeeContext);

    if (!employeeContext) {
        throw new Error(' must be used within an EmployeeProvider');
    }
    const { employees, setEmployees } = employeeContext;

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

    useEffect(() => {
        const loadProjects = async () => {
            const projects = await agent.Project.getAll(searchTerm);
            setFilteredProjects(projects);
        };
        loadProjects();
    }, [searchTerm]);

    const addProject = async (project: ProjectPayload) => {
        const newProject = await agent.Project.add(project);
        setProjects([...projects, newProject]);
        setFilteredProjects([...projects, newProject]);
    };

    const editProject = async (id: number, project: ProjectPayload) => {
        const updatedProject = await agent.Project.update(id, project);
        setProjects(projects.map(proj => (proj.id === id ? updatedProject : proj)));
        setFilteredProjects(projects.map(proj => (proj.id === id ? updatedProject : proj)));
    };

    const addProjectEmployee = async (employee: ProjectEmployee) => {
        await agent.Project.addEmployee(employee);
        setProjects(projects.map(proj =>
            proj.id === employee.projectId ? { ...proj, projectEmployees: [...proj.projectEmployees, employee] } : proj
        ));

        setEmployees(employees.map(emp =>
            emp.id === employee.employeeId ? { ...emp, projectEmployees: [...emp.projectEmployees, employee] } : emp
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

        setEmployees(employees.map(emp =>
            emp.id === employee.employeeId ? {
                ...emp, projectEmployees: emp.projectEmployees.map(mem =>
                    mem.projectId === employee.projectId ? employee : mem
                )
            } : emp
        ));
    };

    return (
        <ProjectContext.Provider value={{ projects, statuses, types, addProject, editProject, addProjectEmployee, editProjectEmployee, setSearchTerm, filteredProjects }}>
            {children}
        </ProjectContext.Provider>
    );
};

export default ProjectContext;