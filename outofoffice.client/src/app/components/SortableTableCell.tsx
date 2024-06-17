import { TableCell, Box } from "@mui/material";
import SortIndicator from "./SortIndicator";

interface Props {
    label: string;
    sortKey: string;
    sortConfig: { key: string; direction: 'asc' | 'desc' };
    handleSort: (key: string) => void;
}

const SortableTableCell = ({ label, sortKey, sortConfig, handleSort }: Props) => (
    <TableCell onClick={() => handleSort(sortKey)}>
        <Box display="flex" alignItems="center">
            {label} <SortIndicator sortConfig={sortConfig} currentKey={sortKey} />
        </Box>
    </TableCell>
);

export default SortableTableCell;