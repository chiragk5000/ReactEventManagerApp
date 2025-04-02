import { Group } from '@mui/icons-material'
import {  Box, Button, Paper, Typography } from '@mui/material'
import { Link } from 'react-router'

export default function Homepage() {
  return (
    <Paper 
    sx={{
      color:'white',
      display:'flex',
      flexDirection:'column',
      gap:6,
      alignItems:'center',
      alignContent:'center',
      justifyContent:'center',
      height:'100vh',
      backgroundImage: 'linear-gradient(135deg, rgb(35, 115, 24) 0%, rgb(0, 0, 0) 69%, rgb(4, 40, 41) 89%)'

    }}
    >
      <Box sx={{
        display:'flex', 
        alignItems:'center' , 
        alignContent :'center',
        color:'white',gap:3
      }}>
        <Group sx={{height:110,width:110}}/>
          <Typography variant='h1'>
          Reactivities
          </Typography>
      </Box>
      <Typography variant='h2'>
        Welcome to Reactivities
      </Typography>
      <Button component= {Link}
        to='/activites'
        size='large'
        variant='contained'
        sx={{height:80,borderRadius:4,fontSize:'1.5rem'}}
        >
        Take me to the activites 
      </Button>
      </Paper>
  )
} 
