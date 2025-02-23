import { useEffect, useState } from "react"

function App() { 
  //const title = 'Welcome to Eventmanagement App !'
  const [activities,setActivities]=useState([]);
  
  useEffect(()=>{
    fetch('http://localhost:5296/api/Activities')
    .then(response => response.json())
    .then (data=> setActivities(data))
  },[])

  return (
    <div>
   <h3 className="app" style={({color:'red'})}>Welcome to EventManager App!</h3>
  <ul>
{
  activities.map((activity)=>(
    <li key={(activity.id)}>{activity.title}</li>
  ))
}
  </ul>
  </div>
  )
}

export default App
