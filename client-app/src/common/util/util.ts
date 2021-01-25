import { IActivity, IAttendee } from "../../app/models/activity";
import { IUser } from "../../app/models/user";

export const combineDateAndTime = (date: Date, time: Date) => {
  const timeString = `${time.getHours()}:${time.getMinutes()}:00`;
  const dateString = `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;

  return new Date(`${dateString} ${timeString}`);
};

export const setActivityProps = (activity: IActivity, currentUser: IUser): void => {
  activity.date = new Date(activity.date);

  activity.isGoing = activity.attendees.some(
    a => a.username === currentUser.username && !a.isHost);
    
  activity.isHost = activity.attendees.some(
    a => a.username === currentUser.username && a.isHost);
}

export const createAttendee = (user: IUser): IAttendee => {
  return {
    displayName: user.displayName,
    isHost: false,
    username: user.username,
    image: user.image!,
  };
}