import { useState } from 'react';

export type SortConfig<T> = {
    key: keyof T | string;
    direction: 'asc' | 'desc';
};

export function useSortableData<T>(items: T[], initialSortConfig: SortConfig<T>, getSortableValue: (item: T, key: keyof T | string) => string | number | undefined) {
    const [sortConfig, setSortConfig] = useState<SortConfig<T>>(initialSortConfig);

    const sortedItems = [...items].sort((a, b) => {
        const aValue = getSortableValue(a, sortConfig.key);
        const bValue = getSortableValue(b, sortConfig.key);

        if (aValue == null && bValue == null) return 0;
        if (aValue == null) return sortConfig.direction === 'asc' ? -1 : 1;
        if (bValue == null) return sortConfig.direction === 'asc' ? 1 : -1;

        if (typeof aValue === 'string' && typeof bValue === 'string') {
            return sortConfig.direction === 'asc' ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue);
        }

        if (aValue < bValue) {
            return sortConfig.direction === 'asc' ? -1 : 1;
        }
        if (aValue > bValue) {
            return sortConfig.direction === 'asc' ? 1 : -1;
        }
        return 0;
    });

    const handleSort = (key: keyof T | string) => {
        let direction: 'asc' | 'desc' = 'asc';
        if (sortConfig.key === key && sortConfig.direction === 'asc') {
            direction = 'desc';
        }
        setSortConfig({ key, direction });
    };

    return { sortedItems, sortConfig, handleSort };
}
