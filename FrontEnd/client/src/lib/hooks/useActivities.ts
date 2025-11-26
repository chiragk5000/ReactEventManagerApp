import { useInfiniteQuery, useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agentapi from "../api/agentapi";
import { useLocation } from "react-router";
import { useAccount } from "./useAcounts";
import { useStore } from "./useStore";

export const useActivites = (id?: string) => {
  //const apiurl = '/Activities';
  const {activityStore : {filter,startDate}}=useStore();
  const { currentUser } = useAccount();
  const queryClient = useQueryClient();
  const location = useLocation();

  // get list 
  const { data: activitiesGroup, isLoading ,isFetchingNextPage,fetchNextPage,hasNextPage } = useInfiniteQuery<PagedList<Activity, string>>({
    queryKey: ['activities',filter,startDate],
    queryFn: async ({ pageParam = null }) => {
      const response = await agentapi.get<PagedList<Activity,string>>('/Activities', {
        params: {
          cursor: pageParam,
          pageSize: 3,
          filter,
          startDate
        }
      });
      console.log("response", response.data)
      return response.data;

    },
   staleTime: 1000 *60 * 5 ,
   initialPageParam:null,
    gcTime: 1000 * 60 * 5,   
    getNextPageParam: (lastPage) => lastPage.nextCursor ? lastPage.nextCursor : undefined,
    enabled: !id && location.pathname === '/activites' && !!currentUser,
    select: data => ({
      ...data,
      pages: data.pages.map((page) => ({
        ...page,
        items: page.items.map(activity => {
          const host = activity.attendees.find(x => x.id === activity.hostId);
          return {
            ...activity,
            isHost: currentUser?.id === activity.hostId,
            isGoing: activity.attendees.some(x => x.id === currentUser?.id),
            hostImageUrl: host?.imageUrl
          }

        })
      }))
    })
    
  });



  // get activity by id 
  const { data: activity, isLoading: isLoadingActivity } = useQuery({
    queryKey: ['activities', id],
    queryFn: async () => {
      const response = await agentapi.get<Activity>(`/Activities/${id}`)
      return response.data;
    },
    enabled: !!id && !!currentUser,
    select: data => {
      const host = data.attendees.find(x => x.id === data.hostId);
      return {
        ...data,
        isHost: currentUser?.id === data.hostId,
        isGoing: data.attendees.some(x => x.id === currentUser?.id),
        hostImageUrl: host?.imageUrl
      }

    }

  })


  // update 
  const updateActivity = useMutation({
    mutationFn: async (activity: Activity) => {
      console.log("Activity", activity)
      await agentapi.put('Activities/EditActivty', activity);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities']
      })
    }

  });



  // post 
  const createActivity = useMutation({
    mutationFn: async (activity: Activity) => {
      //const response = await agentapi.post('/Activities',activity);
      const response = await agentapi.post('Activities/Create', activity);

      return response.data;
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities']
      })
    }

  });

  // delete 
  const deleteActivity = useMutation({
    mutationFn: async (id: string) => {
      await agentapi.delete(`/Activities/${id}`);
    },
    onSuccess: async () => {
      await queryClient.invalidateQueries({
        queryKey: ['activities']
      })
    }

  });



  // for attendance 
  const updateAttendance = useMutation({
    mutationFn: async (id: string) => {
      await agentapi.post(`/Activities/${id}/attend`)
    },
    // Example of optimistic updates 
    onMutate: async (activityId: string) => {
      await queryClient.cancelQueries({ queryKey: ['activities', activityId] });

      const prevActivity = queryClient.getQueryData<Activity>(['activities', activityId]);

      queryClient.setQueryData<Activity>(['activities', activityId], oldActivity => {

        if (!oldActivity || !currentUser) {
          return oldActivity
        }
        const isHost = oldActivity.hostId === currentUser.id;
        const isAttending = oldActivity.attendees.some(x => x.id === currentUser.id);


        return {
          ...oldActivity,
          isCancelled: isHost ? !oldActivity.isCancelled : oldActivity.isCancelled,
          attendees: isAttending
            ? isHost
              ? oldActivity.attendees
              : oldActivity.attendees.filter(x => x.id !== currentUser.id)
            : [...oldActivity.attendees, {
              id: currentUser.id,
              displayName: currentUser.displayName,
              imageUrl: currentUser.imageUrl
            }]


        }

      });
      return { prevActivity };

    },
    onError: (error, activityId, context) => {
      console.log("prevactivity", context?.prevActivity)
      console.log("error inside UpdateAttendance", error)
      if (context?.prevActivity) {
        queryClient.setQueryData(['activities', activityId], context.prevActivity)
      }
    }
    // onSuccess: async () => {
    //       console.log("query id:" + id)

    //   await queryClient.invalidateQueries({
    //     queryKey:['activities',id]
    //   })
    // }
  })


  return {
    activitiesGroup,
    isFetchingNextPage,
    fetchNextPage,
    hasNextPage,
    isLoading,
    updateActivity,
    createActivity,
    deleteActivity,
    activity,
    isLoadingActivity,
    updateAttendance
  }
}