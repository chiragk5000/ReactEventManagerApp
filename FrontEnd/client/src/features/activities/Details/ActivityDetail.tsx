import { Button, Card, CardActions, CardContent, CardMedia, Typography } from "@mui/material"
import { useActivites } from "../../../lib/hooks/useActivities"

type Props = {
    //activity: Activity
    selectedActivity:Activity
    cancelSelectActivity: () => void
    openForm : (id:string) => void
}

export default function ActivityDetail({ selectedActivity, cancelSelectActivity,openForm }: Props) {
   const {activities}= useActivites();
    const activity = activities?.find(x=>x.id==selectedActivity.id);
    
    if(!activity) return <Typography>Loading...</Typography>
    
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
                <Button onClick = {()=>openForm(activity.id)} color="primary">
                    Edit
                </Button>
                <Button onClick={cancelSelectActivity} color="inherit">
                    Cancel
                </Button>
            </CardActions>
        </Card>
    )
}
