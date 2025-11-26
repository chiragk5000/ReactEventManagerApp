import { Box, Container, CssBaseline } from "@mui/material";
import NavBar from "./NavBar";
import { Outlet, ScrollRestoration, useLocation } from "react-router";
import Homepage from "../../features/home/Homepage";
import './style.css';


function App() {
  const location = useLocation();

  return (
    <Box sx={{ bgColor: '#eeeeee' }}>
      <ScrollRestoration/>
      <CssBaseline />
      {
        location.pathname === '/' ? <Homepage /> : (
          <>
            <NavBar />
            <Container maxWidth='xl' sx={{ pt: 14 }}>
              <Outlet />
            </Container>
          </>

        )
      }

    </Box>
  )
}

export default App
