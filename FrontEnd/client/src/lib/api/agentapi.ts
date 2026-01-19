import axios from "axios";
import { store } from "../stores/store";
import { toast } from "react-toastify";
import { router } from "../../app/router/Routes";

const agentapi = axios.create({
  baseURL: import.meta.env.VITE_API_URL
});

// 🔓 Public endpoints (API paths, not UI routes)
const PUBLIC_API_PATHS = [
  "/account/login",
  "/account/register",
  "/account/confirm-email",
  "/account/forgot-password",
  "/account/reset-password"
];

// 🔐 Request interceptor
agentapi.interceptors.request.use(config => {
  const token = localStorage.getItem("jwt");

  // Attach token ONLY if present
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  store.uiStore.isBusy();
  return config;
});

// 💤 Helper
const sleep = (delay: number) =>
  new Promise(resolve => setTimeout(resolve, delay));

// 📥 Response interceptor
agentapi.interceptors.response.use(
  async response => {
    await sleep(300);
    store.uiStore.isIdle();
    return response;
  },
  async error => {
    await sleep(300);
    store.uiStore.isIdle();

    // Network / CORS / server down
    if (!error.response) {
      toast.error("Network error. Please try again.");
      return Promise.reject(error);
    }

    const { status, data, config } = error.response;
    const requestUrl = config?.url ?? "";

    const isPublicApiCall = PUBLIC_API_PATHS.some(path =>
      requestUrl.includes(path)
    );

    const hasToken = !!localStorage.getItem("jwt");

    switch (status) {
      case 400:
        if (data?.errors) {
          const modalErrors: string[] = [];
          for (const key in data.errors) {
            if (data.errors[key]) {
              modalErrors.push(...data.errors[key]);
            }
          }
          throw modalErrors;
        }
        toast.error(data?.title || "Bad request");
        break;

      case 401:
        /**
         * ✅ VERY IMPORTANT LOGIC
         * - No token OR public API → DO NOTHING
         * - Token exists + protected API → REAL session expiry
         */
        if (!hasToken || isPublicApiCall) {
          return Promise.reject(error);
        }
        if(data.any())
        {
          toast.error("Unauthorized")
        }
        if(data.isNotAllowed)
        {
          throw new Error(data.errorMessage)
        }
        
        toast.error("Session expired. Please login again.");

        // Clean logout
        localStorage.removeItem("jwt");
        router.navigate("/login");
        break;

      case 403:
        toast.error("You are not authorized to perform this action.");
        break;

      case 404:
        if (!isPublicApiCall) {
          router.navigate("/not-found");
        }
        break;

      case 500:
        router.navigate("/server-error", { state: { error: data } });
        break;

      default:
        break;
    }

    return Promise.reject(error);
  }
);

export default agentapi;
