import React from 'react';
import { NorthEast, SouthEast } from '@mui/icons-material';

interface SortIndicatorProps {
    sortConfig: { key: string; direction: 'asc' | 'desc' };
    currentKey: string;
}

const SortIndicator: React.FC<SortIndicatorProps> = ({ sortConfig, currentKey }) => {
    if (sortConfig.key === currentKey) {
        return sortConfig.direction === 'asc' ? <NorthEast fontSize="small" /> : <SouthEast fontSize="small" />;
    }
    return null;
};

export default SortIndicator;
