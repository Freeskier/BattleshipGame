import React, { useEffect, useState } from 'react';
import './App.css';
import GamePanel from './Components/GamePanel/GamePanel';
import Header from './Components/Header/Header';
import Login from './Components/Login/Login';
import Register from './Components/Register/Register';

import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";


function App() {
  const [username, setUsername] = useState('');

    return (
      <div>
        <Router>
        <Header username={username} setUsername={setUsername}/>
          <div className="App">
            <Switch>
              <Route exact path={['/','/login']}>
                <Login setName={setUsername}/>
              </Route>
              <Route exact path='/register' component={Register}/>
              <Route exact path='/game' component={GamePanel}/>
            </Switch>
          </div>
        </Router>
      </div>
    );
}

export default App;
