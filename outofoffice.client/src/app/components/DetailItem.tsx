import { useTheme } from "@mui/material";
import { Typography, Box } from "@mui/material";

interface Props {
    label: string;
    value: string | number | undefined | null;
    styles?: object;
}

const DetailItem = ({ label, value, styles }: Props) => {
    const theme = useTheme();
    return (
        (value !== undefined && value !== null && value !== "") &&
        <Typography variant="h5" sx={{ mb: 2, color: theme.palette.text.secondary, ...styles }}>
            {label}: <Box component="span" sx={{ fontWeight: 'bold', color: theme.palette.text.primary }}>{value}</Box>
        </Typography>
    );
};

export default DetailItem;