import { useContext } from 'react';
import { Box, Typography, Button, TableContainer, Paper, Table, TableHead, TableRow, TableCell, TableBody, useTheme } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { SortConfig, useSortableData } from '../../app/hooks/useSortableData';
import SortableTableCell from '../../app/components/SortableTableCell';
import { EditButton, ViewButton } from '../../app/components/ListButtons';
import ProjectContext from '../../app/context/ProjectContext';
import { Project } from '../../app/models/project';
import BoldTableCell from "../../app/components/BoldTableCell";

const ProjectList = () => {
    const theme = useTheme();
    const context = useContext(ProjectContext);
    const navigate = useNavigate();

    if (!context) {
        throw new Error('ProjectList must be used within an ProjectProvider');
    }

    const { projects } = context;

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
    const { sortedItems: sortedProjects, sortConfig, handleSort } = useSortableData(projects, initialSortConfig, getSortableValue);

    function handleAddProject(): void {
        navigate('/projects/new');
    }

    function handleEditProject(id: number): void {
        navigate(`/projects/${id}/edit`);
    }

    function handleViewProject(id: number): void {
        navigate(`/projects/${id}`);
    }

    return (
        <>
            <Box display='flex' justifyContent='space-between'>
                <Typography sx={{ p: 2, fontWeight: 'bold', color: theme.palette.primary.main }} variant='h4'>Projects</Typography>
                <Button onClick={() => handleAddProject()} sx={{ m: 2 }} size='large' variant='contained'>Add</Button>
            </Box>
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
                                    <EditButton id={project.id} handleEdit={handleEditProject} />
                                    <ViewButton id={project.id} handleView={handleViewProject} />
                                </TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
};

export default ProjectList;