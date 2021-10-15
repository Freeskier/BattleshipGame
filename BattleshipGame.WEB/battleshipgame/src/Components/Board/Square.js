import React from 'react';
import './Board.css'

function Square({onClick, type, x, y}) {

    function ColoredSquare(typ) {
        switch (typ.typ) {
            case 0: return (
                <div className='grid-item water' onClick={() => onClick(x, y)}> </div>)
            case 1: return (
                <div className='grid-item black' onClick={() => onClick(x, y)}> </div>)
            case 2: return (
                <div className='grid-item hit' onClick={() => onClick(x, y)}> </div>)
            case 3: return (
                <div className='grid-item sunk onClick={() => onClick(x, y)}'>
                    <div className='line1'/>    
                    <div className='line2'/>    
                </div>)
            default : return (
                <div className='grid-item' onClick={() => onClick(x, y)}></div>)
                
        }
    }


    return (
        <ColoredSquare typ={type}/>
     );
}

export default Square;