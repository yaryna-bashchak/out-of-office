import { useContext, useEffect } from 'react';
import { Box, Typography, Button, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { SortConfig, useSortableData } from '../../app/hooks/useSortableData';
import SortableTableCell from '../../app/components/SortableTableCell';
import { EditButton, ViewButton } from '../../app/components/ListButtons';
import ProjectContext from '../../app/context/ProjectContext';
import { Project } from '../../app/models/project';
import BoldTableCell from "../../app/components/BoldTableCell";
import UserRoleContext from '../../app/context/UserRoleContext';
import SearchLine from '../../app/components/SearchLine';

const ProjectList = () => {
    const theme = useTheme();
    const context = useContext(ProjectContext);
    const userRoleContext = useContext(UserRoleContext);
    const navigate = useNavigate();

    if (!context || !userRoleContext) {
        throw new Error('ProjectList must be used within an ProjectProvider and UserRoleProvider');
    }

    const { filteredProjects, setSearchTerm } = context;
    const { userRole } = userRoleContext;

    useEffect(() => {
        setSearchTerm(undefined);
    }, [setSearchTerm]);

    const getSortableValue = (project: Project, key: string) => {
        switch (key) {
            case 'projectType.name':
                return project.projectType.name;
            case 'status.name':
                return project.status.name;
            case 'projectManager.fullName':
                return project.projectManager.fullName;
            default:
                if (key in project) {
                    const value = project[key as keyof Project];
                    if (typeof value === "number") return value;
                    return value?.toString();
                }
        }
    };

    const initialSortConfig: SortConfig<Project> = { key: 'id', direction: 'asc' };
    const { sortedItems: sortedProjects, sortConfig, handleSort } = useSortableData(filteredProjects, initialSortConfig, getSortableValue);

    function handleAddProject(): void {
        navigate('/projects/new');
    }

    function handleEditProject(id: number): void {
        navigate(`/projects/${id}/edit`);
    }

    function handleViewProject(id: number): void {
        navigate(`/projects/${id}`);
    }

    const handleSearch = (term: string) => {
        setSearchTerm(term);
    };

    return (
        <Box sx={{ minWidth: '800px' }}>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2, fontWeight: 'bold', color: theme.palette.primary.main }} variant='h4'>Projects</Typography>
                {userRole === 'Project Manager' && <Button onClick={() => handleAddProject()} sx={{ m: 2 }} size='large' variant='contained'>Add</Button>}
            </Box>
            <SearchLine handleSearch={handleSearch} label='id' styles={{ mb: '16px' }} />
            {sortedProjects.length === 0 ?
                <Typography>There is no items.</Typography> :
                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <SortableTableCell label="ID" sortKey="id" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="Project Manager" sortKey="projectManager.fullName" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="Start Date" sortKey="startDate" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="End Date" sortKey="endDate" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="Project Type" sortKey="projectType.name" sortConfig={sortConfig} handleSort={handleSort} />
                                <SortableTableCell label="Status" sortKey="status.name" sortConfig={sortConfig} handleSort={handleSort} />
                                <TableCell></TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {sortedProjects.map((project) => (
                                <TableRow key={project.id}>
                                    <TableCell>{project.id} </TableCell>
                                    <BoldTableCell>{project.projectManager.fullName}</BoldTableCell>
                                    <TableCell>{new Date(project.startDate).toLocaleDateString()}</TableCell>
                                    <TableCell>{project.endDate && new Date(project.endDate).toLocaleDateString()}</TableCell>
                                    <TableCell>{project.projectType.name}</TableCell>
                                    <TableCell>{project.status.name}</TableCell>
                                    <TableCell>
                                        {userRole === 'Project Manager' && <EditButton id={project.id} handleEdit={handleEditProject} />}
                                        <ViewButton id={project.id} handleView={handleViewProject} />
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>}
        </Box>
    );
};

export default ProjectList;