import { Box } from '@mui/material'
import ActivityCard from '../ActivityCard'

type Props = {
    acitivites: Activity[]
    selectActivity:(id:string)=> void;

}
export default function ActivityList({ acitivites,selectActivity }: Props) {
    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
            {acitivites.map(activity => (
                <ActivityCard key={activity.id} activity={activity} selectActivity={selectActivity}/>
            ))}
        </Box> 
  )

}
