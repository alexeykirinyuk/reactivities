import { observer } from 'mobx-react-lite';
import React, { useContext } from 'react';
import { Button, Container, Menu } from 'semantic-ui-react';
import { ActivityStoreCtx } from '../../app/stores/activityStore';

const NavBar: React.FC = () => {
  const activityStore = useContext(ActivityStoreCtx);

  return (
    <Menu fixed='top' inverted>
      <Container>
        <Menu.Item header>
          <img src="/assets/logo.png" alt="logo" style={{ marginRight: '10px' }} />
        Reactivities
      </Menu.Item>
        <Menu.Item name='Activities' />
        <Menu.Item>
          <Button positive onClick={activityStore.openCreateForm}>Create Activity</Button>
        </Menu.Item>
      </Container>
    </Menu>
  );
};

export default observer(NavBar);