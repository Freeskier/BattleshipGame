import React, { useState } from 'react';
import './App.css';
import Board from './Components/Board/Board';
import GamePanel from './Components/GamePanel/GamePanel';
import Header from './Components/Header/Header';
import Login from './Components/Login/Login';
import Register from './Components/Register/Register';


function App() {
const [username, setUsername] = useState('Lolasen');

function onLogout(){
  setUsername('');
}

  return (
    <div>
      <Header username={username} onLogout={onLogout}/>
      <div className="App">
        <GamePanel/>
      </div>
    </div>
  );
}

export default App;
