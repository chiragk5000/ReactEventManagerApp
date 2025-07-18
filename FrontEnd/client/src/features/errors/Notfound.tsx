import { SearchOff } from "@mui/icons-material";
import { Button, Paper, Typography } from "@mui/material"
import { Link } from "react-router";

export default function Notfound() {
    return (
        <Paper
            sx={{
                height: 400,
                display: 'flex',
                flexDirection: 'column',
                justifyContent: 'center',
                alignItems: 'center',
                p: 6
            }}
        >
            <SearchOff sx={{fontsize:100}} color='primary'></SearchOff>
            <Typography gutterBottom variant="h3">
                Oops - We could not find what you are looking for 
            </Typography>
            <Button fullWidth component={Link} to='/activites'>
            Return to the activities page
            </Button>
        </Paper>
    );

}  