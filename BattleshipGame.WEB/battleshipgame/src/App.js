import React from 'react';
import './App.css';
import Board from './Components/Board/Board';
import GamePanel from './Components/GamePanel/GamePanel';
import Header from './Components/Header/Header';
import Login from './Components/Login/Login';
import Register from './Components/Register/Register';


function App() {



  return (
    <div>
      <Header/>
      <div className="App">
        <GamePanel/>
      </div>
    </div>
  );
}

export default App;
