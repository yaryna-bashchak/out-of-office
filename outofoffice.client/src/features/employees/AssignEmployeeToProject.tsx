import { useContext } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, Grid, useTheme } from '@mui/material';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import AppTextInput from '../../app/components/AppTextInput';
import AppSelectList from '../../app/components/AppSelectList';
import ProjectContext from '../../app/context/ProjectContext';
import { Project, ProjectEmployee } from '../../app/models/project';

interface TransformedProject {
    id: number;
    name: string;
}

const transformProjects = (projects: Project[]): TransformedProject[] => {
    return projects.map(project => ({
        id: project.id,
        name: `ID=${project.id} PM=${project.projectManager.fullName}${project.comment && ' Comment=' + project.comment}`
    }));
};

const formatDateForInput = (dateString: string) => {
    return dateString.split('T')[0];
};

const formatDateForApi = (dateString: string) => {
    return `${dateString}T00:00:00`;
};

const AssignEmployeeToProject = () => {
 
    const theme = useTheme();
    const employeeContext = useContext(EmployeeContext);
    const projectContext = useContext(ProjectContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!employeeContext || !projectContext) {
        throw new Error('EmployeeEditor must be used within an EmployeeProvider and ProjectProvider');
    }

    const { projects, addProjectEmployee } = projectContext;
    const { employees } = employeeContext;
    const employee = id ? employees.find(emp => emp.id === parseInt(id)) : null;

    const projectEmployee = {
        startDate: '',
        endDate: '',
        employeeId: id ?? 0,
        projectId: 0,
    } as ProjectEmployee;

    const { handleSubmit, control, watch } = useForm<ProjectEmployee>({
        defaultValues: {
            ...projectEmployee,
            startDate: projectEmployee?.startDate ? formatDateForInput(projectEmployee.startDate) : '',
            endDate: projectEmployee?.endDate ? formatDateForInput(projectEmployee.endDate) : '',
        },
        mode: 'onSubmit',
    });

    const onSubmit = async (data: ProjectEmployee) => {
        const transformedData: ProjectEmployee = {
            ...data,
            employeeId: id ? parseInt(id) : 0,
            startDate: formatDateForApi(data.startDate),
            endDate: data.endDate && formatDateForApi(data.endDate),
        };

        await addProjectEmployee(transformedData);
        navigate(`/employees/${id}`);
    };

    const handleCancel = () => {
        navigate('/employees');
    };

    const startDate = watch('startDate');

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h4" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                Assign Employee {employee?.fullName} To Project
            </Typography>
            <form onSubmit={handleSubmit(onSubmit)}>
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <AppSelectList name="projectId" label="Project" control={control} options={transformProjects(projects)} rules={{ validate: value => value !== 0 || 'Project is required' }} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppTextInput type="date" name="startDate" label="Start Date" control={control} rules={{ required: 'Start Date is required' }} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppTextInput type="date" name="endDate" label="End Date" control={control} rules={{
                            validate: value => {
                                if (value) {
                                    return value >= startDate || 'End Date cannot be earlier than Start Date';
                                }
                            }
                        }} />
                    </Grid>
                </Grid>
                <Box sx={{ mt: 4 }}>
                    <Button type="submit" variant="contained" color="primary" sx={{ mr: 2 }}>
                        Save
                    </Button>
                    <Button variant="outlined" color="secondary" onClick={handleCancel}>
                        Cancel
                    </Button>
                </Box>
            </form>
        </Box>
    );
};

export default AssignEmployeeToProject;
