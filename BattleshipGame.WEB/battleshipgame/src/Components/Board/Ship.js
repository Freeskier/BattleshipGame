import React from 'react';
import './Board.css'

function Ship({size, isSunk}) {

    function ShipParts() {
        let createdShip = [];
        for (let index = 0; index < size; index++) {
            createdShip.push(<div
                key = {Math.random() * Date.now()}
                className={!isSunk?'ship-item':'ship-item-sunk'}></div>)
        }
        return createdShip;
    }

    return ( 
        <div className='ship-container'>
            <ShipParts/>
        </div>
     );
}

export default Ship
