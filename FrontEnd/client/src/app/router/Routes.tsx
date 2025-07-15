import { createBrowserRouter } from "react-router";
import App from "../layout/App";
import ActivityDashboard from "../../features/activities/Dashboard/ActivityDashboard";
import ActivityForm from "../../features/activities/Form/ActivityForm";
import Homepage from "../../features/home/Homepage";
import ActivityDetailPage from "../../features/activities/Details/ActivityDetailPage";
import Counter from "../../features/counter/Counter";

export const router  = createBrowserRouter([
    {
        path:'/',
        element:<App />,
        children:[
            {path:'',element:<Homepage/>},
            {path:'activites',element:<ActivityDashboard/>},
            {path:'activites/:id',element:<ActivityDetailPage/>},
            {path:'createActivity',element:<ActivityForm key='create'/>},
            {path:'manage/:id',element:<ActivityForm/>},
            {path:'counter',element:<Counter/>},
        ]
    }
])