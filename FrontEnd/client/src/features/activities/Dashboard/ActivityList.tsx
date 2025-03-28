import { Box, Typography } from '@mui/material'
import ActivityCard from '../ActivityCard'
import { useActivites } from '../../../lib/hooks/useActivities';


export default function ActivityList() {
    
    const { activities,isLoading}=useActivites();

    if(!activities || isLoading) return <Typography>Loading...</Typography>

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
            {activities.map(activity => (
                <ActivityCard key={activity.id} activity={activity}/>
            ))}
        </Box> 
  )

}
