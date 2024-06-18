import { useContext, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Button, Grid, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import AppSelectList from '../../app/components/AppSelectList';
import AppTextInput from '../../app/components/AppTextInput';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Employee } from '../../app/models/employee';
import ProjectContext from '../../app/context/ProjectContext';
import { Project, ProjectPayload, ProjectStatus, ProjectType } from '../../app/models/project';

interface TransformedEmployee {
    id: number;
    name: string;
}

const transformEmployees = (employees: Employee[]): TransformedEmployee[] => {
    return employees.map(employee => ({
        id: employee.id,
        name: employee.fullName
    }));
};

const formatDateForInput = (dateString: string) => {
    return dateString.split('T')[0];
};

const formatDateForApi = (dateString: string) => {
    return `${dateString}T00:00:00`;
};

const ProjectEditor = () => {
    const theme = useTheme();
    const projectContext = useContext(ProjectContext);
    const employeeContext = useContext(EmployeeContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!projectContext || !employeeContext) {
        throw new Error('ProjectList must be used within an ProjectProvider and EmployeeProvider');
    }

    const { projects, types, statuses, editProject, addProject } = projectContext;
    const { projectManagers } = employeeContext;

    const project = id ? projects.find(proj => proj.id === parseInt(id)) : {
        startDate: '',
        endDate: null,
        comment: '',
        projectManager: { id: 0, fullName: '' } as Employee,
        projectType: { id: 0, name: '' } as ProjectType,
        status: { id: 0, name: '' } as ProjectStatus,
    } as Project;

    const { handleSubmit, control, reset, watch } = useForm<Project>({
        defaultValues: {
            ...project,
            startDate: project?.startDate ? formatDateForInput(project.startDate) : '',
            endDate: project?.endDate ? formatDateForInput(project.endDate) : null,
        },
        mode: 'onSubmit',
    });

    useEffect(() => {
        if (id) {
            reset({
                ...project,
                startDate: project?.startDate ? formatDateForInput(project.startDate) : '',
                endDate: project?.endDate ? formatDateForInput(project.endDate) : null,
            });
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [reset, project]);

    const onSubmit = async (data: Project) => {
        const transformedData: ProjectPayload = {
            startDate: formatDateForApi(data.startDate),
            endDate: data.endDate && formatDateForApi(data.endDate),
            comment: data.comment,
            projectTypeId: data.projectType.id,
            projectManagerId: data.projectManager.id,
            statusId: data.status.id,
        };
        if (id) {
            await editProject(parseInt(id), transformedData);
        } else {
            await addProject(transformedData);
        }
        navigate('/projects');
    };

    const handleCancel = () => {
        navigate('/projects');
    };

    const startDate = watch('startDate');

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h4" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                {id ? 'Edit Project' : 'Add Project'}
            </Typography>
            <form onSubmit={handleSubmit(onSubmit)}>
                <Grid container spacing={3}>
                    <Grid item xs={12} sm={6}>
                        <AppSelectList name="projectType.id" label="Project Type" control={control} options={types} rules={{ validate: value => value !== 0 || 'Project Type is required' }} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppSelectList name="projectManager.id" label="Project Manager" control={control} options={transformEmployees(projectManagers)} rules={{ validate: value => value !== 0 || 'Project Manager is required' }} />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <AppTextInput type="date" name="startDate" label="Start Date" control={control} rules={{ required: 'Start Date is required' }} />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <AppTextInput type="date" name="endDate" label="End Date" control={control} rules={{
                            validate: value => {
                                if (value) {
                                    return value >= startDate || 'End Date cannot be earlier than Start Date';
                                }
                            }
                        }} />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <AppSelectList name="status.id" label="Status" control={control} options={statuses} rules={{ validate: value => value !== 0 || 'Status is required' }} />
                    </Grid>
                    <Grid item xs={12}>
                        <AppTextInput name="comment" label="Comment" multiline={true} rows={4} control={control} />
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

export default ProjectEditor;

