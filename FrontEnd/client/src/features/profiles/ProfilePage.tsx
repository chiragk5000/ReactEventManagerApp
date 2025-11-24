import Grid2 from '@mui/material/Grid2';
import ProfileHeader from './ProfileHeader';
import ProfileContent from './ProfileContent';
import { useParams } from 'react-router';
import { useProfile } from '../../lib/hooks/useProfile';
import { Typography } from '@mui/material';

export default function ProfilePage() {
  const {id} = useParams();
  const{profile,loadingProfile} = useProfile(id);

  if(loadingProfile) return <Typography> Loading profile...</Typography>
    if(!profile) return <Typography> Profile not found...</Typography>
  return (
    <Grid2 container spacing={2}>
      <Grid2 size={12}>
        <ProfileHeader  />
        <ProfileContent />
      </Grid2>
    </Grid2>
  );
}
