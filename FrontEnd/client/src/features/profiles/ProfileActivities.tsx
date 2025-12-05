import  { SyntheticEvent, useEffect, useState } from 'react';
import { useParams, Link } from 'react-router';
import { useProfile } from '../../lib/hooks/useProfile';
import { Box, Card, CardMedia, Typography, Tabs, Tab, CardContent, Grid2 } from '@mui/material';
import { format } from 'date-fns';

export default function ProfileActivities() {
  const [activeTab, setActiveTab] = useState(0);
  const { id } = useParams();
  const { userActivities, setfilter, loadingUserActivities } = useProfile(id);

  useEffect(() => {
    setfilter('future');
  }, [setfilter]);

  const tabs = [
    { menuItem: 'Future Events', key: 'future' },
    { menuItem: 'Past Events', key: 'past' },
    { menuItem: 'Hosting', key: 'hosting' },
  ];

  const handleTabChange = (_: SyntheticEvent, newValue: number) => {
    setActiveTab(newValue);
    setfilter(tabs[newValue].key);
  };

  return (
    <Box>
      <Grid2 container spacing={2}>
        <Grid2 size={12}>
          <Tabs value={activeTab} onChange={handleTabChange}>
            {tabs.map((tab, index) => (
              <Tab label={tab.menuItem} key={index} />
            ))}
          </Tabs>
        </Grid2>
        </Grid2>

        {/* No Activities Message */}
        {!loadingUserActivities &&
          userActivities &&
          userActivities.length === 0 && (
            <Typography mt={2}>No activities to show</Typography>
          )}

        {/* Activities List */}
        <Grid2
          container
          spacing={2}
          sx={{ marginTop: 2, height: 400, overflow: 'auto' }}
        >
          {userActivities &&
            userActivities.map((activity: Activity) => (
              <Grid2 size={2} key={activity.id}>
                <Link to={`/activites/${activity.id}`} style={{ textDecoration: 'none' }}>
                  <Card elevation={4}>
                    <CardMedia
                      component="img"
                      height="100"
                      image={`/Images/CategoryImages/${activity.category}.jpg`}
                      alt={activity.title}
                      sx={{ objectFit: 'cover' }}
                    />

                    <CardContent>
                      <Typography variant="h6" textAlign="center" mb={1}>
                        {activity.title}
                      </Typography>

                      <Typography
                        variant="body2"
                        textAlign="center"
                        display="flex"
                        flexDirection="column"
                      >
                        <span>{format(activity.date, 'do LLL yyyy')}</span>
                        <span>{format(activity.date, 'h:mm a')}</span>
                      </Typography>
                    </CardContent>
                  </Card>
                </Link>
              </Grid2>
            ))}
        </Grid2>
    </Box>
  );
}
