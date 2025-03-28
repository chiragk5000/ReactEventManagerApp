import Group from "@mui/icons-material/Group";
import { Box, AppBar, Toolbar, Typography,  Container, MenuItem } from "@mui/material";
import { NavLink } from "react-router";
import MenuItemlink from "../shared/component/MenuItemlink";

// type Props = {
//     openForm : () => void
// }
export default function NavBar() {
    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="static" sx={{ backgroundImage: 'linear-graident(135deg,rgb(115, 24, 86) 0%, #218aae 69%, #20a7ac 89% )' }}>
                <Container maxWidth='xl'>
                    <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
                        <Box>
                            <MenuItem component={NavLink} to='/' sx={{ display: 'flex', gap: 2 }}>
                                <Group fontSize="large" />
                                <Typography variant="h4" fontWeight='bold'>Reactivities</Typography>
                            </MenuItem>
                        </Box>
                        <Box>
                            <MenuItemlink  to='/activites' >
                                Activities
                            </MenuItemlink>
                        </Box>
                        <Box>
                            <MenuItemlink
                                 to='/createActivity'
                                >
                                Create Activity
                            </MenuItemlink>
                        </Box>
                        {/* <Box>
                            <MenuItem sx={{ fontSize: '1.2rem', textTransform: 'uppercase', fontWeight: 'bold' }}>
                                Contact
                            </MenuItem>
                        </Box> */}
                        <MenuItem>
                        User menu
                        </MenuItem>
                    </Toolbar>
                </Container>
            </AppBar>
        </Box>
    );
}
