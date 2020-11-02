import React, { Component } from "react";
import "./App.css";
import axios from "axios";
import "semantic-ui-css/semantic.min.css";
import { Header, Icon, List } from "semantic-ui-react";

interface Item {
  id: number;
  name: string;
}

interface AppState {
  values: Item[];
}

class App extends Component {
  state: AppState = {
    values: [],
  };

  componentDidMount() {
    this.fetchValues();
  }

  async fetchValues() {
    const response = await axios.get<Item[]>(
      "http://localhost:5000/api/values"
    );
    this.setState({
      values: response.data,
    });
  }

  render() {
    return (
      <div>
        <Header as="h2">
          <Icon name="plug" />
          <Header.Content>Reactivities</Header.Content>
        </Header>
        <List>
          {this.state.values.map((value) => (
            <List.Item key={value.id}>{value.name}</List.Item>
          ))}
        </List>
      </div>
    );
  }
}

export default App;
