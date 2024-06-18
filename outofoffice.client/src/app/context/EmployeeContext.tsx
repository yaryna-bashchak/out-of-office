import { createContext, useState, useEffect, ReactNode } from 'react';
import { Employee, EmployeePayload, EmployeeStatus, Position, Subdivision } from '../models/employee';
import agent from '../api/agent';

interface EmployeeContextType {
  employees: Employee[];
  positions: Position[];
  statuses: EmployeeStatus[];
  subdivisions: Subdivision[];
  addEmployee: (employee: EmployeePayload) => Promise<void>;
  editEmployee: (id: number, employee: EmployeePayload) => Promise<void>;
  hrManagers: Employee[];
  projectManagers: Employee[];
}

const EmployeeContext = createContext<EmployeeContextType | undefined>(undefined);

interface EmployeeProviderProps {
  children: ReactNode;
}

export const EmployeeProvider = ({ children }: EmployeeProviderProps) => {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [hrManagers, setHrManagers] = useState<Employee[]>([]);
  const [projectManagers, setProjectManagers] = useState<Employee[]>([]);
  const [positions, setPositions] = useState<Position[]>([]);
  const [statuses, setStatuses] = useState<EmployeeStatus[]>([]);
  const [subdivisions, setSubdivisions] = useState<Subdivision[]>([]);

  useEffect(() => {
    const loadEmployees = async () => {
      const employees = await agent.Employee.getAll();
      setEmployees(employees);
    };
    loadEmployees();

    const loadPositions = async () => {
      const positions = await agent.Employee.getPositions();
      setPositions(positions);
    };
    loadPositions();

    const loadStatuses = async () => {
      const statuses = await agent.Employee.getStatuses();
      setStatuses(statuses);
    };
    loadStatuses();

    const loadSubdivisions = async () => {
      const subdivisions = await agent.Employee.getSubdivisions();
      setSubdivisions(subdivisions);
    };
    loadSubdivisions();

    const loadHrManagers = async () => {
      const hrManagers = await agent.Employee.getHRManagers();
      setHrManagers(hrManagers);
    };
    loadHrManagers();

    const loadProjectManagers = async () => {
      const projectManagers = await agent.Employee.getProjectManagers();
      setProjectManagers(projectManagers);
    };
    loadProjectManagers();
  }, []);

  const addEmployee = async (employee: EmployeePayload) => {
    const newEmployee = await agent.Employee.add(employee);
    setEmployees([...employees, newEmployee]);

    if (newEmployee.position.name === 'HR Manager') {
      setHrManagers([...hrManagers, newEmployee]);
    }
    if (newEmployee.position.name === 'Project Manager') {
      setProjectManagers([...projectManagers, newEmployee]);
    }
  };

  const editEmployee = async (id: number, employee: EmployeePayload) => {
    const updatedEmployee = await agent.Employee.update(id, employee);
    setEmployees(employees.map(emp => (emp.id === id ? updatedEmployee : emp)));

    if (updatedEmployee.position.name === 'HR Manager') {
      setHrManagers(hrManagers.map(emp => (emp.id === id ? updatedEmployee : emp)));
    } else {
      setHrManagers(hrManagers.filter(emp => emp.id !== id));
    }

    if (updatedEmployee.position.name === 'Project Manager') {
      setProjectManagers(projectManagers.map(emp => (emp.id === id ? updatedEmployee : emp)));
    } else {
      setProjectManagers(projectManagers.filter(emp => emp.id !== id));
    }
  };

  return (
    <EmployeeContext.Provider value={{ employees, positions, statuses, subdivisions, addEmployee, editEmployee, hrManagers, projectManagers }}>
      {children}
    </EmployeeContext.Provider>
  );
};

export default EmployeeContext;

