import React from 'react';
import Board from '../Board/Board';
import './GamePanel.css'

function GamePanel() {


    return ( 
        <div className='game-panel-container'>
            <div className='boards-container'>
                <div className='board-container'>
                    <h1>Your board </h1>
                    <Board/>
                </div>
                <div className='board-container'>
                    <h1>Enemy's board </h1>
                    <Board/>
                </div>
            </div>
        </div>
     );
}

export default GamePanel;