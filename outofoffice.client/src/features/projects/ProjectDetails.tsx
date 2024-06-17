import { useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Box, Typography, Button, useTheme, TableContainer, Paper, Table, TableBody, TableCell, TableHead, TableRow } from '@mui/material';
import DetailItem from '../../app/components/DetailItem';
import ProjectContext from '../../app/context/ProjectContext';
import BoldTableCell from "../../app/components/BoldTableCell";

const ProjectDetails = () => {
    const theme = useTheme();
    const context = useContext(ProjectContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!context) {
        throw new Error('ProjectDetails must be used within a ProjectProvider');
    }

    const { projects } = context;
    const project = id ? projects.find(proj => proj.id === parseInt(id)) : null;

    const handleGoBack = () => {
        navigate('/projects');
    };

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h3" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                Project Details
            </Typography>
            <DetailItem label='Project Type' value={project?.projectType.name} />
            <DetailItem label='Project Manager' value={project?.projectManager.fullName} />
            <DetailItem label='Start Date' value={project ? new Date(project.startDate).toLocaleDateString() : null} />
            <DetailItem label='End Date' value={project && project.endDate ? new Date(project.endDate).toLocaleDateString() : null} />
            <DetailItem label='Comment' value={project?.comment} />
            <DetailItem label='Status' value={project?.status.name} />
            <DetailItem label='Team Members' value={project?.members.length} />
            {project && project.members.length > 0 && <TableContainer component={Paper} sx={{ mb: 2 }}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Member ID</TableCell>
                            <TableCell>Full Name</TableCell>
                            <TableCell>Subdivision</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {project?.members.map((employee) => (
                            <TableRow key={employee.id}>
                                <TableCell>{employee.id} </TableCell>
                                <BoldTableCell>{employee.fullName}</BoldTableCell>
                                <TableCell>{employee.subdivision.name}</TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>}
            <Button variant="outlined" onClick={handleGoBack} >
                Back to Projects List
            </Button>
        </Box>
    );
};

export default ProjectDetails;
