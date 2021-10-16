import React from 'react';
import Board from '../Board/Board';
import Stats from '../Board/Stats';
import Chat from '../Chat/Chat';
import './GamePanel.css'

function GamePanel() {


    return ( 
        <div className='game-panel-container'>
            <div className='boards-container'>
                <div className='board-container'>
                    <h1>Your board </h1>
                    <div className='board-with-stats'>
                        <Board/>
                        <Stats/>
                    </div>
                </div>
                <div className='board-container'>
                    <h1>Enemy's board </h1>
                    <div className='board-with-stats'>
                        <Board/>
                        <Stats/>
                    </div>
                </div>
            </div>
            <Chat/>
        </div>
     );
}

export default GamePanel;