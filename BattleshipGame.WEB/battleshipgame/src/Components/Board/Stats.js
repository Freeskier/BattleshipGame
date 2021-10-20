import React from 'react';
import './Board.css'
import Ship from './Ship'

function Stats({data}) {
    const mappedShips = data.map((s, k) => { 
        return <Ship 
            size={s.size} 
            hitCount={s.hitCount}
            key={k}/>}
    )
    return ( 
        <div className='stats'>
            {mappedShips}
        </div>
     );
}

export default Stats;