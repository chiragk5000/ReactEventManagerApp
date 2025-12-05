import { useAccount } from '../../lib/hooks/useAcounts'
import { Navigate, Outlet, useLocation } from 'react-router';
import { Typography } from '@mui/material';

export default function RequireAuth() {

    const {currentUser,loadingUserInfo}=useAccount();
    const location = useLocation();

    if(loadingUserInfo) return <Typography>Loading...</Typography>
    if(!currentUser) return <Navigate replace to='/login' state={{from:location}}/>

  return (
   <Outlet/>
  )
}
