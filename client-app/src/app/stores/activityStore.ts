import { action, makeObservable, observable } from "mobx";
import { createContext } from "react";
import agent from "../api/agent";
import { IActivity } from "../models/activity";

class ActivityStore {
  @observable public activities: IActivity[] = [];
  @observable public loadingInitial = false;

  constructor() {
    makeObservable(this);
  }

  @action public loadActivities = () => {
    this.loadingInitial = true;

    agent.Activities.list()
      .then((activities) => {
        activities.forEach((activity) => {
          activity.date = activity.date.split(".")[0];
          this.activities.push(activity);
        });
      })
      .finally(() => (this.loadingInitial = false));
  };
}

export default createContext(new ActivityStore());
