import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agentapi from "../api/agentapi";

export const useActivites = (id?:string) => {
  //const apiurl = '/Activities';
  const queryClient = useQueryClient();

  // get list 
  const { data: activities, isLoading } = useQuery({
    queryKey: ['activities'],
    queryFn: async () => {
      const response = await agentapi.get<Activity[]>('Activities');
      //console.log("response",response.data)
          return response.data;
          
    }
  });

  // update 
  const updateActivity = useMutation({
    mutationFn:async (activity:Activity)=>{
      await agentapi.put('/Activities',activity);
    },
    onSuccess:async ()=>{
      await queryClient.invalidateQueries({
        queryKey:['activities']
      })
    }

  });

  // get activity by id 
  const {data:activity,isLoading:isLoadingActivity}=useQuery({
    queryKey:['activites',id],
    queryFn:async()=>{
      const response = await agentapi.get<Activity>(`/Activities/${id}`)
      return response.data;
    },
    enabled:!!id

  })

  // post 
  const createActivity = useMutation({
    mutationFn:async (activity:Activity)=>{
      const response = await agentapi.post('/Activities',activity);
      return response.data;
    },
    onSuccess:async ()=>{
      await queryClient.invalidateQueries({
        queryKey:['activities']
      })
    }
    
  });

  // delete 
  const deleteActivity = useMutation({
    mutationFn:async (id:string)=>{
      await agentapi.delete(`/Activities/${id}`);
    },
    onSuccess:async ()=>{
      await queryClient.invalidateQueries({
        queryKey:['activities']
      })
    }

  });


  return {
    activities,
    isLoading,
    updateActivity,
    createActivity,
    deleteActivity,
    activity,
    isLoadingActivity
  }
}