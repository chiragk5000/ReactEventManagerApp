import { HubConnection, HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";
import { useEffect, useRef } from "react";
import { useLocalObservable } from "mobx-react-lite";
import { runInAction } from "mobx";

export const useComments = (activityId?: string) => {

  const created = useRef(false);
  const token = localStorage.getItem("jwt");
  const commentStore = useLocalObservable(() => ({
    comments: [] as ChatComments[],
    hubConnection: null as HubConnection | null,
     
    createHubConnection(activityId:string){
      if(!activityId) return;
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(`${import.meta.env.VITE_COMMENTS_URL}?activityId=${activityId}`,
          {
            
             accessTokenFactory: () => token!
          })
          .withAutomaticReconnect()
          .build();

          this.hubConnection.start().catch(error=>console.log('Error establishing in connection',error));
          this.hubConnection.on('LoadComments',comments=>{
            runInAction(()=>{
              this.comments=comments
            })
            
          });
            this.hubConnection.on('ReciveComment',comment=>{
              runInAction(()=>{
                this.comments.unshift(comment);
              })
            
          })
    },

    stopHubConnection(){
      if (this.hubConnection?.state === HubConnectionState.Connected){
          this.hubConnection.stop().catch(error=>console.log('Error stopping connection',error));
      }
    }
    



    
  }));


  useEffect(() => {
    if (activityId && !created.current) {
      created.current = true;
      commentStore.createHubConnection(activityId);
    }

    return () => {
      commentStore.stopHubConnection();
      commentStore.comments=[];

    }

  }, [activityId,commentStore]);


  return {commentStore};
};
