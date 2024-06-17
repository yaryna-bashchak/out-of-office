import { FormControl, InputLabel, Select, MenuItem, FormHelperText } from '@mui/material';
import { FieldValues, UseControllerProps, useController } from 'react-hook-form';

interface Option {
    id: number;
    name: string;
}

interface Props<T extends FieldValues> extends UseControllerProps<T> {
    label: string;
    options: Option[];
}
export default function AppSelectList<T extends FieldValues>(props: Props<T>) {
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