import axios from "axios";


const agentapi = axios.create({
    baseURL: import.meta.env.VITE_API_URL
 
    
});
console.log("agentapi",agentapi);
const sleep = (delay: number) => {
    return new Promise(resolve => {
        setTimeout(resolve, delay);
    })
}

agentapi.interceptors.response.use(async response => {
    try {
        await sleep(1000);
        console.log("response1",response)
        return response;
    }
    catch (error) {
        console.log(error);
        return Promise.reject(error);

    }
})

export default agentapi;