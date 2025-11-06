import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import agentapi from "../api/agentapi"
import { useMemo } from "react";

export const useProfile = (id?: string) => {
    const queryClient = useQueryClient();

    // Get profile
    const { data: profile, isLoading: loadingProfile } = useQuery<Profile>({
        queryKey: ['profile', id],
        queryFn: async () => {
            const response = await agentapi.get<Profile>(`/profiles/${id}`);
            return response.data
        },
        enabled: !!id


    });

    // Get Photos
    const { data: photos, isLoading: loadingPhotos } = useQuery<Photo[]>({
        queryKey: ['photos', id],
        queryFn: async () => {
            const response = await agentapi.get<Photo[]>(`/profiles/${id}/photos`);
            return response.data
        },
        enabled: !!id


    });


    // Current user or another user 

    const isCurrentUser = useMemo(() => {
        return id === queryClient.getQueryData<User>(['user'])?.id


    }, [id, queryClient]);

    // Upload photo
    const uploadPhoto = useMutation({
        mutationFn: async (file: Blob) => {
            const formData = new FormData();
            formData.append('file', file);
            const response = await agentapi.post('/profiles/add-photo', formData, {
                headers: { 'Content-Type': 'multipart/form-data' }
            });
            return response.data;
        },
        onSuccess: async (photo: Photo) => {
            await queryClient.invalidateQueries({
                queryKey: ['photos', id]
            });
            queryClient.setQueryData<User>(['user'], (data) => {
                if (!data) return data;
                return {
                    ...data,
                    imageUrl: data.imageUrl ?? photo.url
                }

            });
            queryClient.setQueryData<Profile>(['profile', id], (data) => {
                if (!data) return data;
                return {
                    ...data,
                    imageUrl: data.imageUrl ?? photo.url
                }

            });
        }
    });



    /// Setting the main photo

    const setMainPhoto = useMutation({
        mutationFn: async (photo: Photo) => {
            await agentapi.put(`/profiles/${photo.id}/setMain`)
        },
        onSuccess: (_, photo) => {
            queryClient.setQueryData<User>(['user'], (userData) => {
                if (!userData) return userData
                return {
                    ...userData,
                    imageUrl: photo.url
                }
            });
            queryClient.setQueryData<Profile | undefined>(['profile', id], (profile) => {
                if (!profile) return profile;
                return {
                    ...profile,
                    imageUrl: photo.url
                };
            });
        }
    })

    // delete photo
    const deletePhoto = useMutation({
  mutationFn: async (photoId: string) => {
    await agentapi.delete(`/profiles/${photoId}/photos`);
  },
  onSuccess: (_, photoId) => {
    queryClient.setQueryData<Photo[] | undefined>(['photos', id], (photos) => {
      if (!photos) return photos;
      return photos.filter(x => x.id !== photoId);
    });
  },
});





    return {
        profile,
        loadingProfile,
        photos,
        loadingPhotos,
        isCurrentUser,
        uploadPhoto,
        setMainPhoto,
        deletePhoto
    }
}