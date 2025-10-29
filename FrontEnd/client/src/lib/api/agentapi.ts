import axios from "axios";
import { store } from "../stores/store";
import { toast } from "react-toastify";
import { router } from "../../app/router/Routes";


const agentapi = axios.create({
    baseURL: import.meta.env.VITE_API_URL
 
    
});


// Attach JWT to all requests
agentapi.interceptors.request.use((config) => {
  const token = localStorage.getItem("jwt");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});


//console.log("agentapi",agentapi);
const sleep = (delay: number) => {
    return new Promise(resolve => {
        setTimeout(resolve, delay);
    })
}

agentapi.interceptors.request.use(config => {
store.uiStore.isBusy();
return config;
})

agentapi.interceptors.response.use(async response => {
    // try {
    //     await sleep(1000);
    //     //console.log("response1",response)
    //     return response;
    // }
    // catch (error) {
    //     console.log(error);
    //     return Promise.reject(error);

    // }
    // finally{
    //     store.uiStore.isIdle();
    await sleep(1000);
    store.uiStore.isIdle();
    return response;
    
    },
    async error =>{
        await sleep(1000);
        store.uiStore.isIdle();
        
    const {status,data} =error.response;
    switch(status){
        case 400:
           if (data.errors){
            const modalStateErrors =[];
            for (const key in data.errors){
                if (data.errors[key])
                {
                    modalStateErrors.push(data.errors[key]);
                }
            }
            throw modalStateErrors.flat();
            
           }
           else {
                  toast.error(data);  
            }

            break;
             case 401:
            toast.error('Unauthorized');
            break;
            case 404:
            //toast.error('Not found');
            router.navigate('/not-found')
            break;
            case 500:
            //toast.error('Server error');
            router.navigate('/server-error',{state:{error:data}})
            break;
            default:
                break;


    }

        return Promise.reject(error);

    }
)

export default agentapi;