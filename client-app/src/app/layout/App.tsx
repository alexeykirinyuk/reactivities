import React, {
  useEffect,
  Fragment,
  useContext,
} from "react";
import "semantic-ui-css/semantic.min.css";
import { Container } from "semantic-ui-react";
import NavBar from "../../features/nav/NavBar";
import ActivityDashboard from "../../features/activities/dashboard/ActivityDashboard";
import LoadingComponent from "./LoadingComponent";
import { observer } from "mobx-react-lite";
import { ActivityStoreCtx } from "../stores/activityStore";

const App = () => {
  const activityStore = useContext(ActivityStoreCtx);

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]);

  if (activityStore.loadingInitial) {
    return <LoadingComponent content="Loading Activities..." inverted />;
  }

  return (
    <Fragment>
      <NavBar />
      <Container style={{ marginTop: "7em" }}></Container>
      <div style={{ marginLeft: "7em", marginRight: "7em" }}>
        <ActivityDashboard />
      </div>
    </Fragment>
  );
};

export default observer(App);
