import { createContext, useState, useEffect, ReactNode } from 'react';
import { Employee } from '../models/employee';
import agent from '../api/agent';

interface EmployeeContextType {
  employees: Employee[];
  addEmployee: (employee: Employee) => Promise<void>;
  editEmployee: (id: number, employee: Employee) => Promise<void>;
}

const EmployeeContext = createContext<EmployeeContextType | undefined>(undefined);

interface EmployeeProviderProps {
  children: ReactNode;
}

export const EmployeeProvider = ({ children }: EmployeeProviderProps) => {
  const [employees, setEmployees] = useState<Employee[]>([]);

  useEffect(() => {
    const loadEmployees = async () => {
        const employees = await agent.Employee.getAll();
        setEmployees(employees);
    };
    loadEmployees();
  }, []);

  const addEmployee = async (employee: Employee) => {
    const newEmployee = await agent.Employee.add(employee);
    setEmployees([...employees, newEmployee]);
  };

  const editEmployee = async (id: number, employee: Employee) => {
    const updatedEmployee = await agent.Employee.update(id, employee);
    setEmployees(employees.map(emp => (emp.id === id ? updatedEmployee : emp)));
  };

  return (
    <EmployeeContext.Provider value={{ employees, addEmployee, editEmployee }}>
      {children}
    </EmployeeContext.Provider>
  );
};

export default EmployeeContext;

