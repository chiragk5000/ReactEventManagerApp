import { List, ListItem, Typography } from "@mui/material";
import {  useEffect, useState } from "react"
import axios from 'axios'

function App() { 
  //const title = 'Welcome to Eventmanagement App !'
  const [activities,setActivities]=useState<Activity[]>([]);

  const url = 'https://localhost:7006/api/Activities';
  // useEffect(()=>{
  //   fetch('https://localhost:7006/api/Activities')
  //   .then(response => response.json())
  //   .then (data=> setActivities(data))
  // },[])

// Using  axios 
  useEffect(()=>{
    axios.get<Activity[]>(url)
    .then(response => setActivities(response.data))
    return () =>{}
  },[])

  return (
    <>
    <Typography variant="h3">Welcome to EventManager App!</Typography>
   {/* <h3 className="app" style={({color:'red'})}>Welcome to EventManager App!</h3> */}
  <List>
{
  activities.map((activity)=>(
    <ListItem key={(activity.id)}>{activity.title}</ListItem>
  ))
}
  </List>
  </>
  )
}

export default App
