import { useContext, useEffect } from 'react';
import EmployeeContext from '../../app/context/EmployeeContext';
import { Box, Typography, Button, Grid } from '@mui/material';
import { Employee, EmployeePayload } from '../../app/models/employee';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import AppTextInput from '../../app/components/AppTextInput';
import AppSelectList from '../../app/components/AppSelectList';

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

const EmployeeEditor = () => {
    const context = useContext(EmployeeContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!context) {
        throw new Error('EmployeeEditor must be used within an EmployeeProvider');
    }

    const { employees, positions, statuses, subdivisions, addEmployee, editEmployee, hrManagers } = context;
    const employee = id ? employees.find(emp => emp.id === parseInt(id)) : {
        fullName: '',
        outOfOfficeBalance: 0,
        position: { id: 0, name: '' },
        status: { id: 0, name: '' },
        subdivision: { id: 0, name: '' },
        peoplePartner: { id: 0, fullName: '' },
    } as Employee;
    const { handleSubmit, control, reset } = useForm<Employee>({
        defaultValues: employee,
    });

    useEffect(() => {
        if (id) {
            reset(employee);
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [reset, employee]);

    const onSubmit = async (data: Employee) => {
        const transformedData: EmployeePayload = {
            fullName: data.fullName,
            outOfOfficeBalance: data.outOfOfficeBalance,
            positionId: data.position.id,
            statusId: data.status.id,
            subdivisionId: data.subdivision.id,
            photo: null,
            peoplePartnerId: data.peoplePartner ? data.peoplePartner.id : null
        };
        if (id) {
            await editEmployee(parseInt(id), transformedData);
        } else {
            await addEmployee(transformedData);
        }
        navigate('/employees');
    };

    const handleCancel = () => {
        navigate('/employees');
    };

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h4" gutterBottom sx={{ mb: 4 }}>
                {id ? 'Edit Employee' : 'Add Employee'}
            </Typography>
            <form onSubmit={handleSubmit(onSubmit)}>
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <AppTextInput name="fullName" label="Full Name" control={control} />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <AppSelectList name="position.id" label="Position" control={control} options={positions} />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <AppSelectList name="subdivision.id" label="Subdivision" control={control} options={subdivisions} />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <AppSelectList name="status.id" label="Status" control={control} options={statuses} />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <AppSelectList name="peoplePartner.id" label="People Partner" control={control} options={transformEmployees(hrManagers)} />
                    </Grid>
                    <Grid item xs={12} sm={4}>
                        <AppTextInput type="number" name="outOfOfficeBalance" label="Out-Of-Office Balance" control={control} />
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

export default EmployeeEditor;
