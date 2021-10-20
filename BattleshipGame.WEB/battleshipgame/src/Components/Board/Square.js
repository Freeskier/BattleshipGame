import React from 'react';
import './Board.css'

function Square({onClick, isEnemy, type, x, y}) {

    function ColoredSquare(typ) {
        switch (typ.typ) {
            case 0: return (
                <div className={'grid-item water' + (isEnemy? ' grid-item-not-enemy':'')}
                    onClick={(isEnemy?() => onClick(x, y):function(){})}> </div>)
            case 1: return (
                <div className='shot'> </div>)
            case 2: return (
                <div className='black'> </div>)
            case 3: return (
                <div className='hit'> </div>)
            case 4: return (
                <div className='sunk'>
                    <div className='line1'/>    
                    <div className='line2'/>    
                </div>)
            default : return (
                <div className='grid-item'></div>)
                
        }
    }


    return (
        <ColoredSquare typ={type}/>
     );
}

export default Square;