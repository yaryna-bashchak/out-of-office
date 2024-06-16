import { FormControl, InputLabel, Select, MenuItem } from '@mui/material';
import { UseControllerProps, useController } from 'react-hook-form';
import { Employee } from '../models/employee';

interface Option {
    id: number;
    name: string;
}

interface Props extends UseControllerProps<Employee> {
    label: string;
    options: Option[];
}

export default function AppSelectList(props: Props) {
    const { fieldState, field } = useController({ ...props, defaultValue: '' })
    const { label, options } = props;

    return (
        <FormControl fullWidth variant="outlined" error={!!fieldState.error}>
            <InputLabel>{label}</InputLabel>
            <Select {...field} label={label}>
                {options.map(option => (
                    <MenuItem key={option.id} value={option.id}>
                        {option.name}
                    </MenuItem>
                ))}
            </Select>
        </FormControl>
    )
}