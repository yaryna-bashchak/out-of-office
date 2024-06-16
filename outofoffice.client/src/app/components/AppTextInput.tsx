import { TextField } from "@mui/material";
import { useController, UseControllerProps } from "react-hook-form";
import { Employee } from "../models/employee";

interface Props extends UseControllerProps<Employee> {
    label: string;
    multiline?: boolean;
    rows?: number;
    type?: string;
}

export default function AppTextInput(props: Props) {
    const {fieldState, field} = useController({...props, defaultValue: ''})

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
        />
    )
}