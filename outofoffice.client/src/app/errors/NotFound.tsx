import { Button, Divider, Typography } from "@mui/material";
import { Link } from "react-router-dom";

type Props = {
    message?: string;
};

export default function NotFound({ message = 'Oops! We could not find such a page :(' }: Props) {
    return (
        <>
            <Typography gutterBottom variant="h4">{message}</Typography>
            <Divider />
            <Button fullWidth component={Link} to="/">Back to Home Page</Button>
        </>
    )
}