import { TextField, debounce } from "@mui/material";
import { useState } from "react";

interface Props {
    handleSearch: (term: string) => void;
    label: string;
    styles?: object;
}

const SearchLine = ({ handleSearch, label, styles }: Props) => {
    const [term, setTerm] = useState('');

    const debouncedSearch = debounce((event) => {
        handleSearch(event.target.value);
    }, 1000)

    return (
        <TextField
            value={term}
            size="small"
            onChange={(e) => {
                setTerm(e.target.value);
                debouncedSearch(e);
            }}
            placeholder={"Search by " + label + "..."}
            sx={{...styles}}
        />
    );
};

export default SearchLine;