import { Box, Container, CssBaseline } from "@mui/material";
import NavBar from "./NavBar";
import { Outlet, useLocation } from "react-router";
import Homepage from "../../features/home/Homepage";
import './style.css'; 


function App() {
  const location = useLocation();
  
  return (
    <Box sx={{ bgColor: '#eeeeee' }}>
      <CssBaseline />
      {
        location.pathname === '/' ? <Homepage/> :(
<>
<NavBar />
      <Container maxWidth='xl' sx={{ mt: 3 }}>
        <Outlet />
      </Container>
</>

        )
      }
      
    </Box>
  )
}

export default App
