import {
  action,
  makeObservable,
  observable,
  computed,
  configure,
  runInAction,
} from "mobx";
import { createContext, SyntheticEvent } from "react";
import agent from "../api/agent";
import { IActivity } from "../models/activity";

configure({ enforceActions: "always" });
class ActivityStore {
  @observable public activityRegistry = new Map<string, IActivity>();
  @observable public activity: IActivity | undefined;
  @observable public editMode = false;
  @observable public loadingInitial = false;
  @observable public submitting = false;
  @observable public target = "";

  constructor() {
    makeObservable(this);
  }

  @computed public get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => Date.parse(a.date) - Date.parse(b.date)
    );
  }

  @action public loadActivities = async () => {
    this.loadingInitial = true;

    try {
      const activities = await agent.Activities.list();
      runInAction(() => {
        activities.forEach((activity) => {
          activity.date = activity.date.split(".")[0];
          this.activityRegistry.set(activity.id, activity);
        });
      });
    } catch (error) {
      console.log(error);
    } finally {
      runInAction(() => {
        this.loadingInitial = false;
      });
    }
  };

  @action public selectActivity = (id: string) => {
    this.activity = this.activityRegistry.get(id);
    this.editMode = false;
  };

  @action loadActivity = async (id: string) => {
    let activity = this.getActivity(id);
    if (activity) {
      this.activity = activity;
    } else {
      this.loadingInitial = true;
      try {
        activity = await agent.Activities.details(id);
        runInAction(() => {
          this.activity = activity;
        });
      } catch (error) {
        console.log(error);
      } finally {
        runInAction(() => {
          this.loadingInitial = false;
        });
      }
    }
  }

  getActivity = (id: string): IActivity | undefined => {
    return this.activityRegistry.get(id);
  }

  @action public createActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      await agent.Activities.create(activity);
      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.editMode = false;
      });
    } catch (error) {
      console.log(error);
    } finally {
      runInAction(() => {
        this.submitting = false;
      });
    }
  };

  @action editActivity = async (activity: IActivity) => {
    this.submitting = true;
    try {
      await agent.Activities.update(activity);
      runInAction(() => {
        this.activityRegistry.set(activity.id, activity);
        this.activity = activity;
        this.editMode = false;
      });
    } catch (err) {
      console.log(err);
    } finally {
      runInAction(() => {
        this.submitting = false;
      });
    }
  };

  @action deleteActivity = async (
    event: SyntheticEvent<HTMLButtonElement>,
    id: string
  ): Promise<void> => {
    this.submitting = true;
    this.target = event.currentTarget.name;
    try {
      await agent.Activities.delete(id);
      runInAction(() => {
        this.activityRegistry.delete(id);
      });
    } catch (err) {
      console.log(err);
    } finally {
      runInAction(() => {
        this.submitting = false;
      });
    }
  };

  @action openEditForm = (id: string) => {
    this.activity = this.activityRegistry.get(id);
    this.editMode = true;
  };

  @action cancelSelectedActivity = () => {
    this.activity = undefined;
  };

  @action cancelFormOpen = () => {
    this.editMode = false;
  };

  @action public openCreateForm = () => {
    this.editMode = true;
    this.activity = undefined;
  };
}

export const ActivityStoreCtx = createContext(new ActivityStore());
