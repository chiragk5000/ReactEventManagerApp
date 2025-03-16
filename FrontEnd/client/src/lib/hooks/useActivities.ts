import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agentapi from "../api/agentapi";

export const useActivites = () => {
  //const apiurl = '/Activities';
  const queryClient = useQueryClient();

  const { data: activities, isLoading } = useQuery({
    queryKey: ['activities'],
    queryFn: async () => {
      const response = await agentapi.get<Activity[]>('Activities');
      console.log("response",response.data)
          return response.data;
          
    }
  });

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

  const createActivity = useMutation({
    mutationFn:async (activity:Activity)=>{
      await agentapi.post('/Activities',activity);
    },
    onSuccess:async ()=>{
      await queryClient.invalidateQueries({
        queryKey:['activities']
      })
    }
    
  });

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
    deleteActivity
  }
}