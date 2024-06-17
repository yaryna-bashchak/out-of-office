import { TextField } from "@mui/material";
import { FieldValues, useController, UseControllerProps } from "react-hook-form";

interface Props<T extends FieldValues> extends UseControllerProps<T> {
    label: string;
    multiline?: boolean;
    rows?: number;
    type?: string;
}

export default function AppTextInput<T extends FieldValues>(props: Props<T>) {
    const { fieldState, field } = useController({ ...props })

    return (
        <TextField
            {...props}
            {...field}
            multiline={props.multiline}
            rows={props.rows}
            type={props.type}
            fullWidth
            variant="outlined"
            error={!!fieldState.error}
            helperText={fieldState.error ? fieldState.error.message : ''}
        />
    )
}