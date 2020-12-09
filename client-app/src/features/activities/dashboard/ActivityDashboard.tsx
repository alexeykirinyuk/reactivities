import React from "react";
import { Grid } from "semantic-ui-react";
import ActivityList from "./ActivityList";

const ActivityDashboard: React.FC = () => {
  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityList />
      </Grid.Column>
      <Grid.Column width={6}>
        <div>Activity filters</div>
      </Grid.Column>
    </Grid>
  );
};

export default ActivityDashboard;
