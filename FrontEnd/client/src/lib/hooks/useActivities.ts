import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agentapi from "../api/agentapi";
import { useLocation } from "react-router";
import { useAccount } from "./useAcounts";

export const useActivites = (id?:string) => {
  //const apiurl = '/Activities';
  const {currentUser} = useAccount();
  const queryClient = useQueryClient();
  const location = useLocation();

  // get list 
  const { data: activities, isLoading } = useQuery({
    queryKey: ['activities'],
    queryFn: async () => {
      const response = await agentapi.get<Activity[]>('Activities');
      //console.log("response",response.data)
          return response.data;
          
    },
    enabled:!id && location.pathname ==='/activites' && !!currentUser
    //staleTime: 1000 *60 * 5 
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
    enabled:!!id && !!currentUser

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