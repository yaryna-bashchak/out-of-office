import { useContext, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import { Box, Button, Grid, Typography } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import AppSelectList from '../../app/components/AppSelectList';
import AppTextInput from '../../app/components/AppTextInput';
import EmployeeContext from '../../app/context/EmployeeContext';
import LeaveRequestContext from '../../app/context/LeaveRequestContext';
import { Employee } from '../../app/models/employee';
import { AbsenceReason, RequestType, LeaveRequestStatus, LeaveRequest, LeaveRequestPayload } from '../../app/models/leaveRequest';

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

const LeaveRequestEditor = () => {
    const theme = useTheme();
    const leaveRequestContext = useContext(LeaveRequestContext);
    const employeeContext = useContext(EmployeeContext);
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();

    if (!leaveRequestContext || !employeeContext) {
        throw new Error('LeaveRequestEditor must be used within a LeaveRequestProvider and EmployeeProvider');
    }

    const { leaveRequests, absenceReasons, types, addLeaveRequest, editLeaveRequestInfo } = leaveRequestContext;
    const { employees } = employeeContext;

    const leaveRequest = id ? leaveRequests.find(req => req.id === parseInt(id)) : {
        startDate: '',
        endDate: '',
        hours: null,
        comment: '',
        absenceReason: { id: 0, name: '' } as AbsenceReason,
        employee: { id: 0, fullName: '' } as Employee,
        requestType: { id: 0, name: '' } as RequestType,
        status: { id: 0, name: '' } as LeaveRequestStatus,
    } as LeaveRequest;

    const { handleSubmit, control, reset, watch } = useForm<LeaveRequest>({
        defaultValues: {
            ...leaveRequest,
            startDate: leaveRequest?.startDate ? formatDateForInput(leaveRequest.startDate) : '',
            endDate: leaveRequest?.endDate ? formatDateForInput(leaveRequest.endDate) : '',
        },
        mode: 'onSubmit',
    });

    useEffect(() => {
        if (id) {
            reset({
                ...leaveRequest,
                startDate: leaveRequest?.startDate ? formatDateForInput(leaveRequest.startDate) : '',
                endDate: leaveRequest?.endDate ? formatDateForInput(leaveRequest.endDate) : '',
            });
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [reset, leaveRequest]);

    const onSubmit = async (data: LeaveRequest) => {
        const transformedData: LeaveRequestPayload = {
            startDate: formatDateForApi(data.startDate),
            endDate: formatDateForApi(selectedTypeName === 'Full days' ? data.endDate : data.startDate),
            hours: selectedTypeName === 'Full days' ? null : data.hours,
            comment: data.comment,
            absenceReasonId: data.absenceReason.id,
            employeeId: data.employee.id,
            requestTypeId: data.requestType.id,
        };
        if (id) {
            await editLeaveRequestInfo(parseInt(id), transformedData);
        } else {
            await addLeaveRequest(transformedData);
        }
        navigate('/leave-requests');
    };

    const handleCancel = () => {
        navigate('/leave-requests');
    };

    const selectedType = watch('requestType.id');
    const selectedTypeName = types.find(type => type.id === selectedType)?.name;
    const startDate = watch('startDate');

    return (
        <Box sx={{ p: 4 }}>
            <Typography variant="h4" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
                {id ? 'Edit Leave Request' : 'Add Leave Request'}
            </Typography>
            <form onSubmit={handleSubmit(onSubmit)}>
                <Grid container spacing={3}>
                    <Grid item xs={12}>
                        <AppSelectList name="employee.id" label="Employee" control={control} options={transformEmployees(employees)} rules={{ validate: value => value !== 0 || 'Employee is required' }} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppSelectList name="absenceReason.id" label="Absence Reason" control={control} options={absenceReasons} rules={{ validate: value => value !== 0 || 'Absence Reason is required' }} />
                    </Grid>
                    <Grid item xs={12} sm={6}>
                        <AppSelectList name="requestType.id" label="Request Type" control={control} options={types} rules={{ validate: value => value !== 0 || 'Request Type is required' }} />
                    </Grid>
                    {selectedTypeName === 'Full days' && (
                        <>
                            <Grid item xs={12} sm={6}>
                                <AppTextInput type="date" name="startDate" label="Start Date" control={control} rules={{ required: 'Start Date is required' }} />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                                <AppTextInput type="date" name="endDate" label="End Date" control={control} rules={{
                                    required: 'End Date is required',
                                    validate: value => {
                                        if (!value) {
                                            return 'End Date is required';
                                        }
                                        return value >= startDate || 'End Date cannot be earlier than Start Date';
                                    }
                                }} />
                            </Grid>
                        </>
                    )}
                    {selectedTypeName === 'Partial day' && (
                        <>
                            <Grid item xs={12} sm={6}>
                                <AppTextInput type="date" name="startDate" label="Start Date" control={control} rules={{ required: 'Start Date is required' }} />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                                <AppTextInput type="number" name="hours" label="Hours" control={control} rules={{
                                    required: 'Hours is required',
                                    min: {
                                        value: 1,
                                        message: 'Hours must be greater than 0'
                                    },
                                    max: {
                                        value: 8,
                                        message: 'Hours must be less than or equal to 8'
                                    }
                                }} />
                            </Grid>
                        </>
                    )}
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

export default LeaveRequestEditor;

