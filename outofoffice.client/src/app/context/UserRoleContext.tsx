import { Dispatch, SetStateAction, createContext, ReactNode, useState } from "react";

interface UserRoleContextType {
    userRole: 'HR Manager' | 'Project Manager' | 'Employee';
    setUserRole: Dispatch<SetStateAction<'HR Manager' | 'Project Manager' | 'Employee'>>;
}

const UserRoleContext = createContext<UserRoleContextType | undefined>(undefined);

interface UserRoleProviderProps {
    children: ReactNode;
}

export const UserRoleProvider = ({ children }: UserRoleProviderProps) => {
    const [userRole, setUserRole] = useState<'HR Manager' | 'Project Manager' | 'Employee'>('Employee');

    return (
        <UserRoleContext.Provider value={{ userRole, setUserRole }}>
            {children}
        </UserRoleContext.Provider>
    );
};

export default UserRoleContext;