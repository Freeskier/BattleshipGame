import React from 'react';
import './Board.css'
import Ship from './Ship'

function Stats() {
    return ( 
        <div className='stats'>
            <Ship size={4}/>
            <Ship size={3}/>
            <Ship size={3}/>
            <Ship size={2} isSunk={true}/>
            <Ship size={2}/>
            <Ship size={2}/>
            <Ship size={1}/>
            <Ship size={1}/>
            <Ship size={1}/>
            <Ship size={1}/>
        </div>
     );
}

export default Stats;