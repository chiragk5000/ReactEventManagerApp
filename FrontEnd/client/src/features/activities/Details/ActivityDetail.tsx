import { Button, Card, CardActions, CardContent, CardMedia, Typography } from "@mui/material"
import { Link, useNavigate, useParams } from "react-router";
import { useActivites } from "../../../lib/hooks/useActivities";



export default function ActivityDetail() {
    const navigate = useNavigate();
    const { id } = useParams(); // to amtch from routes.tsx file 

    const { activity, isLoadingActivity } = useActivites(id)


    if (isLoadingActivity) return <Typography>Loading...</Typography>

    if (!activity) return <Typography>Activity not found !!!</Typography>


    return (
        <Card sx={{ borderRadius: 3 }}>
            <CardMedia component='img'
                src={`/Images/CategoryImages/${activity.category}.jpg`}
            />
            <CardContent>
                <Typography variant="h5">{activity.title}</Typography>
                <Typography variant="subtitle1" fontWeight='light'>{activity.date}</Typography>
                <Typography variant="body1">{activity.description}</Typography>

            </CardContent>
            <CardActions>
                <Button component={Link} to={`/manage/${activity.id}`} color="primary">
                    Edit
                </Button>
                <Button onClick={() => navigate('/activites')} color="inherit">
                    Cancel
                </Button>
            </CardActions>
        </Card>
    )
}
