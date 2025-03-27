import { Grid2 } from "@mui/material/";
import ActivityList from "./ActivityList";
import ActivityDetail from "../Details/ActivityDetail";
import ActivityForm from "../Form/ActivityForm";

type Props = {
    activities: Activity[]
    selectActivity: (id: string) => void;
    cancelSelectActivity: () => void;
    selectedActivity?: Activity;
    openForm: (id: string) => void;
    closeForm: () => void;
    editMode: boolean;
    
}
export default function ActivityDashboard({ activities, selectActivity, cancelSelectActivity,
    selectedActivity, openForm, closeForm, editMode }: Props) {
    return (
        <Grid2 container spacing={3}>
            <Grid2 size={7}>
                {/* <List>
                    {
                        props.activities.map((activity) => (
                            <ListItem key={(activity.id)}>
                                <ListItemText>{activity.title}</ListItemText>
                            </ListItem>

                        ))
                    }
                </List> */}
                <ActivityList acitivites={activities}
                    selectActivity={selectActivity} />
            </Grid2>
            <Grid2 size={5}>
                {
                    selectedActivity && !editMode &&
                    <ActivityDetail selectedActivity={selectedActivity}
                        cancelSelectActivity={cancelSelectActivity}
                        openForm={openForm}
                    />
                }
                {editMode &&
                    <ActivityForm closeForm={closeForm} activity={selectedActivity}  />}
            </Grid2>

        </Grid2>
    )
}
