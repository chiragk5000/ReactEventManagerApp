import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { LoginSchema } from "../schemas/loginSchema"
import agentapi from "../api/agentapi"
import {  useLocation, useNavigate } from "react-router";
import { RegisterSchema } from "../schemas/registerSchema";
import { toast } from "react-toastify";





export const useAccount = () => {
    const queryClient = useQueryClient();
    const navigate = useNavigate();
    const location = useLocation(); // 

   // to detect if user is login or register page
    const isAuthPage =    location.pathname === "/login" || location.pathname === "/register";
    
    // for login 
    const loginUser = useMutation({
        mutationFn: async (creds: LoginSchema) => {
            const response = await agentapi.post<{ token: string }>("account/login", creds);
            localStorage.setItem("jwt", response.data.token); // save token
        },
        onSuccess: async () => {
            await queryClient.invalidateQueries({ queryKey: ["user"] });
            //await navigate('/activites');
            
        },
    });

    // for logout 
    const logoutUser = useMutation({
        mutationFn :async()=>{

            await agentapi.post('/account/logout');
        },
        onSuccess:()=>{
             localStorage.removeItem("jwt");
            queryClient.removeQueries({queryKey:['user']});
            queryClient.removeQueries({queryKey:['activities']});

            navigate('/');
        }
    })


    // for registerUser
    const registerUser =useMutation({
        mutationFn:async (creds:RegisterSchema)=>{
            await agentapi.post('/account/register',creds)
        },
        onSuccess:()=>{
            toast.success('Register succesful- you can now login');
            navigate('/login');
        }
    })

    // for currentuser
    const { data: currentUser ,isLoading:loadingUserInfo } = useQuery({
        queryKey: ['user'],
        queryFn: async () => {

            const response = await agentapi.get<User>('/account/user-info-token');
            return response.data

        },
        enabled : !queryClient.getQueryData(['user']) && !isAuthPage

    })


    return {
        loginUser,
        currentUser,
        logoutUser,
        loadingUserInfo,
        registerUser
    }
}