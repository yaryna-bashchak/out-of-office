import { FormControl, InputLabel, Select, MenuItem, FormHelperText } from '@mui/material';
import { UseControllerProps, useController } from 'react-hook-form';
import { Employee } from '../models/employee';
// import { useEffect } from 'react';

interface Option {
    id: number;
    name: string;
}

interface Props extends UseControllerProps<Employee> {
    label: string;
    options: Option[];
}
export default function AppSelectList(props: Props) {
    const { fieldState, field } = useController({ ...props });
    const { label, options } = props;

    return (
        <FormControl fullWidth variant="outlined" error={!!fieldState.error}>
            <InputLabel>{label}</InputLabel>
            <Select
                {...field}
                label={label}
                displayEmpty                
            >
                {options.map(option => (
                    <MenuItem key={option.id} value={option.id}>
                        {option.name}
                    </MenuItem>
                ))}
            </Select>
            {fieldState.error && <FormHelperText>{fieldState.error.message}</FormHelperText>}
        </FormControl>
    );
}