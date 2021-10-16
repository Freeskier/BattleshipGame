import React from 'react';
import './Board.css';
import Square from './Square';

function Board({isDisabled}) {


    const squaresData = [
        ['', ' '],
        [1, 'A'],
        [2, 'B'],
        [3, 'C'],
        [4, 'D'],
        [5, 'E'],
        [6, 'F'],
        [7, 'G'],
        [8, 'H'],
        [9, 'I'],
        [10, 'J']];

    const exampleData = [
        [0, 1, 2, 3, 2, 1, 0, 0, 1, 3],
        [0, 1, 2, 3, 2, 1, 1, 0, 1, 3],
        [0, 1, 2, 2, 2, 1, 0, 0, 1, 3],
        [0, 1, 2, 3, 2, 1, 0, 0, 1, 3],
        [0, 1, 2, 3, 2, 1, 2, 3, 1, 3],
        [0, 0, 2, 0, 2, 1, 0, 0, 1, 3],
        [0, 1, 2, 3, 2, 1, 0, 0, 1, 3],
        [0, 1, 2, 3, 2, 1, 0, 2, 1, 3],
        [0, 1, 0, 3, 2, 1, 0, 0, 1, 3],
        [0, 1, 2, 3, 2, 1, 0, 0, 1, 3]
    ];

    

    function onSquareClick(x, y) {
        console.log(x)
    }

    const mergedData = squaresData.map(
        (square, index) => {
            return squaresData.map(
                (square2, index2) => {
                    if(index===0 && index2 === 0)
                        return <div 
                        className='empty'
                        key = {Math.random() * Date.now()}></div>
                    if(index2 === 0)
                        return <div
                        key = {Math.random() * Date.now()}
                         className='outter-grid-item'>{squaresData[index][0]}</div>
                    if(index === 0) 
                        return <div 
                        key = {Math.random() * Date.now()}
                        className='outter-grid-item'>{squaresData[index2][1]}</div>
                    return <Square x={index - 1} y={index2 - 1} 
                        onClick={onSquareClick} 
                        type={exampleData[index-1][index2-1]}
                        key = {Math.random() * Date.now()}/>
                }
            )
        }

    );

    return ( 
        <>
        <div className='grid-container'>
            {mergedData}
            <div className={isDisabled?'disable':''} ></div>
        </div> 
        </>
    );
}

export default Board;