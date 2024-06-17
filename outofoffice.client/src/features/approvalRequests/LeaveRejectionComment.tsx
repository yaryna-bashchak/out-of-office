import { Box, Button, Typography, useTheme } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import AppTextInput from "../../app/components/AppTextInput";
import { useContext } from "react";
import ApprovalRequestContext from "../../app/context/ApprovalRequestContext";
import { ApprovalRequestPayload } from "../../app/models/approvalRequest";
import { useForm } from "react-hook-form";
import DetailItem from "../../app/components/DetailItem";

const LeaveRejectionComment = () => {
    const theme = useTheme();
    const { id } = useParams<{ id: string | undefined }>();
    const navigate = useNavigate();
    const context = useContext(ApprovalRequestContext);

    if (!context) {
        throw new Error('LeaveRejectionComment must be used within an ApprovalRequestProvider');
    }

    const { approvalRequests, statuses, editApprovalRequest } = context;
    const approvalRequest = id ? approvalRequests.find(req => req.id === parseInt(id)) : null;

    const { handleSubmit, control } = useForm<ApprovalRequestPayload>({
        defaultValues: {},
        mode: 'onSubmit',
    });

    const onSubmit = async (data: ApprovalRequestPayload) => {
        const status = statuses.find(s => s.name === "Rejected");
        if (status && id) {
            const transformedData: ApprovalRequestPayload = {
                statusId: status.id,
                comment: data.comment,
            };
            await editApprovalRequest(parseInt(id), transformedData);
        }
        navigate('/approval-requests');
    };

    const handleCancel = () => {
        navigate('/approval-requests');
    }

    return (<Box sx={{ p: 4 }}>
        <Typography variant="h4" sx={{ mb: 4, fontWeight: 'bold', color: theme.palette.primary.main }}>
            Leave Rejection Comment (optional)
        </Typography>
        <DetailItem label="Approval Request ID" value={approvalRequest?.id} />
        <DetailItem label="Employee" value={approvalRequest?.leaveRequest.employee.fullName} />
        <form onSubmit={handleSubmit(onSubmit)}>
            <AppTextInput name="comment" label="Comment" multiline={true} rows={4} control={control} />
            <Box sx={{ mt: 4 }}>
                <Button type="submit" variant="contained" color="secondary" sx={{ mr: 2 }}>
                    Confirm Rejection
                </Button>
                <Button variant="outlined" color="secondary" onClick={handleCancel}>
                    Cancel
                </Button>
            </Box>
        </form>
    </Box>
    )
}

export default LeaveRejectionComment;