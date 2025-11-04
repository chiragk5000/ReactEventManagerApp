import { Box, Button, Paper,  Typography } from '@mui/material'
import { useActivites } from '../../../lib/hooks/useActivities';
import {  useNavigate, useParams } from 'react-router';
import {  useForm } from 'react-hook-form';
import { useEffect } from 'react';
import { activitySchema, ActivitySchema } from '../../../lib/schemas/activitySchema';
import { zodResolver} from '@hookform/resolvers/zod';
import TextInput from '../../../app/shared/component/TextInput';
import { categoryOptions } from './categoryOptions';
import SelectInput from '../../../app/shared/component/SelectInput';
import DateTimeInput from '../../../app/shared/component/DateTimeInput';
import LocationInput from '../../../app/shared/component/LocationInput';



export default function ActivityForm() {
    const { control,reset,handleSubmit} = useForm<ActivitySchema>({
        mode:'onTouched', // for validating without clicking the button
        resolver :zodResolver(activitySchema),
         defaultValues: {
    title: '',
    description: '',
    category: '',
    date: '',
    city: '',
    venue: '',
  
  },
    });
    const { id } = useParams();
    const { updateActivity, createActivity, activity, isLoadingActivity } = useActivites(id);
    //const activity = {} as Activity
    // helper function to handle forms 

    useEffect(() => {
        if (activity) reset ({
            ...activity,
            location:{
                city:activity.city,
                venue:activity.venue,
                latittude:activity.latitude,
                longitude:activity.longitude

            }
        });
    },[activity,reset])

    const navigate = useNavigate();

    const onSubmit =  async (data:ActivitySchema) => {

        
        const {location, ...rest}=data;
        const actualdata = {...rest, ...location};
        try{
            if(activity){
                updateActivity.mutate({...activity, ...actualdata},{
                    
                    onSuccess:() =>  navigate(`/activites/${activity.id}`)

                })
            }
            else{
                createActivity.mutate(actualdata,{
                    onSuccess:(data) => navigate(`/activites/${data}`)
                })
            }

        }
        catch (error){
            console.log(error)
        }

        // event.preventDefault();
        // const formData = new FormData(event.currentTarget);

        // const data: { [key: string]: FormDataEntryValue } = {}

        // formData.forEach((value, key) => {
        //     data[key] = value;
        // });

        // if (activity) {
        //     data.id = activity.id;
        //     await updateActivity.mutateAsync(data as unknown as Activity);
        //     navigate(`/activites/${activity.id}`);

        // }

        // else {
        //     createActivity.mutate(data as unknown as Activity, {
        //         onSuccess: (data) => {
        //             navigate(`/activites/${data.id}`)
        //         }
        //     });

        // }

        if (isLoadingActivity) return <Typography>Loading activity...</Typography>


        //submitForm(data as unknown as Activity);
    }

    return (
        <Paper sx={{ borderRadius: 3, padding: 3 }}>
            <Typography variant='h5' gutterBottom color='primary'>
                {activity ? 'Edit Activity ' : 'Create Activity '}
            </Typography>
            <Box component='form' onSubmit={handleSubmit(onSubmit)} display='flex' flexDirection='column' gap={3}>
            <TextInput label='Title'  control={control} name='title'/> 
            <TextInput label='Description' control={control} name='description' multiline rows={3}/>
            <Box display='flex' gap={3}>
                <SelectInput items={categoryOptions} label='Category' control={control} name='category'/>
            <DateTimeInput label='Date' control={control} name='date'/>

            </Box>
            
            <LocationInput control={control} label='Enter the location' name ='location'/>
                {/* <TextField {...register('title')} 
                label='Title' defaultValue={activity?.title} error = {!!errors.title} helperText={errors.title?.message}/>
                <TextField {...register('description')} label='Description' defaultValue={activity?.description} multiline rows={3} />
                <TextField {...register('category')} label='Category' defaultValue={activity?.category} />
                <TextField {...register('date')} label='Date' type='date'
                    defaultValue={activity?.date
                        ? new Date(activity.date).toISOString().split('T')[0]
                        : new Date().toISOString().split('T')[0]
                    } />
                <TextField {...register('city')} label='City' defaultValue={activity?.city} />
                <TextField {...register('venue')} label='Venue' defaultValue={activity?.venue} /> */}
                <Box display='flex' justifyContent='end' gap={3}>
                    <Button color='inherit'>
                        Cancel
                    </Button>
                    <Button type="submit" color='success' variant='contained'
                        disabled={updateActivity.isLoading || createActivity.isLoading}>
                        Submit
                    </Button>
                </Box>
            </Box>
        </Paper>
    )
}

