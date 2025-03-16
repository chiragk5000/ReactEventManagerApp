import { Box, Container, CssBaseline, Typography } from "@mui/material";
import {  useState } from "react"
import NavBar from "./NavBar";
import ActivityDashboard from "../../features/activities/Dashboard/ActivityDashboard";
import {  useActivites } from "../../lib/hooks/useActivities";

function App() {
  //const title = 'Welcome to Eventmanagement App !'
  //const [activities, setActivities] = useState<Activity[]>([]);
  
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  const [editMode, setEditMode] = useState(false);
  const { activities,isLoading}=useActivites();
  // useEffect(()=>{
  //   fetch('https://localhost:7006/api/Activities')
  //   .then(response => response.json())
  //   .then (data=> setActivities(data))
  // },[])


  // Using  axios 
  // useEffect(() => {
  //   axios.get<Activity[]>(apiurl)
  //     .then(response => setActivities(response.data))
  //   return () => { }
  // }, [])


  // instead of use effect usign query client tanstack


  const handleSelectActivity = (id: string) => {
    setSelectedActivity(activities!.find(x => x.id === id));
  }

  const handleCancelActivity = () => {
    setSelectedActivity(undefined);
  }

  const handleOpenForm = (id?: string) => {
    if (id) handleSelectActivity(id);
    else handleCancelActivity();
    setEditMode(true);
  }
  const handleCloseForm = () => {
    setEditMode(false);
  }

  /*
  const handleSubmitForm = (activity: Activity) => {
    if (activity.id) {
      setActivities(activities.map(x => x.id === activity.id ? activity : x))
    }
    else {
      const newActivity = { ...activity, id: activities.length.toString() }
      setSelectedActivity(newActivity);
      setActivities([...activities, newActivity])
      console.log("a", newActivity);
    }
    console.log(activity);
    setEditMode(false);

  }
    */


  // const handleDeleteActivity = (id: string) => {
  //   //setActivities(activities.filter(x => x.id !== id));
  //   console.log(id);
  // }

  return (
    <Box sx={{ bgColor: '#eeeeee' }}>
      <CssBaseline />
      <NavBar openForm={handleOpenForm} />
      <Container maxWidth='xl' sx={{ mt: 3 }}>
        {/* <Typography variant="h3">Welcome to EventManager App!</Typography> */}
        {/* <h3 className="app" style={({color:'red'})}>Welcome to EventManager App!</h3> */}

        {!activities || isLoading ? (
          <Typography>Loading...</Typography>

        ) : (
          <ActivityDashboard activities={activities}
            selectActivity={handleSelectActivity}
            cancelSelectActivity={handleCancelActivity}
            selectedActivity={selectedActivity}
            editMode={editMode}
            openForm={handleOpenForm}
            closeForm={handleCloseForm}
           
            
          />


        )}


      </Container>
    </Box>
  )
}

export default App
