import { Dispatch, SetStateAction, createContext, ReactNode, useState } from "react";

// eslint-disable-next-line react-refresh/only-export-components
export const roles: string[] = ['HR Manager', 'Project Manager', 'Employee'] as const;
export type UserRole = (typeof roles)[number];

interface UserRoleContextType {
    userRole: UserRole;
    setUserRole: Dispatch<SetStateAction<UserRole>>;
}

const UserRoleContext = createContext<UserRoleContextType | undefined>(undefined);

interface UserRoleProviderProps {
    children: ReactNode;
}

export const UserRoleProvider = ({ children }: UserRoleProviderProps) => {
    const [userRole, setUserRole] = useState<UserRole>('Employee');

    return (
        <UserRoleContext.Provider value={{ userRole, setUserRole }}>
            {children}
        </UserRoleContext.Provider>
    );
};

export default UserRoleContext;