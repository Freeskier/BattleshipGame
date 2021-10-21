import React, {useEffect, useState} from 'react';
import Board from '../Board/Board';
import Stats from '../Board/Stats';
import Chat from '../Chat/Chat';
import './GamePanel.css'
import { HubConnectionBuilder } from '@microsoft/signalr';
import { toast } from 'react-toastify';
import {useHistory} from 'react-router-dom'
import ChallengeModal from '../Chat/ChallengeModal';


function GamePanel() {
    let history = useHistory();

    const [connection, setConnection] = useState(null);
    const [myBoard, setMyBoard] = useState([]);
    const [enemyBoard, setEnemyBoard] = useState([]);
    const [myShipStats, setMyShipStats] = useState([]);
    const [enemyStats, setEnemyStats] = useState([]);
    const [roomID, setRoomID] = useState('');
    const [openResponseModal, setOpenResponseModal] = useState(false);
    const [challengingUser, setChallengingUser] = useState('');
    const [isMyMove, setIsMyMove] = useState(false);
    const [showBoards, setShowBoards] = useState(false);
    const [autoPlay, setAutoPlay] = useState(false);
    const [update, setUpdate] = useState(false);



    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/hub/game', { accessTokenFactory: () => localStorage.getItem('token') })
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
        newConnection.serverTimeoutInMilliseconds = 100000;
    }, []);

    useEffect(() =>{
        if(isMyMove && update && autoPlay) {
            setTimeout(() => {
                onSquareClick(1, 1);
                setUpdate(false);
            }, 500)
        }
    }, [update, setUpdate, isMyMove])

    useEffect(() => {
        let unmounted = false;
        if (connection) {
            connection.start()
            .then(result => {    
                connection.on('ChallengeUserCallback', challenge => {
                    //if(unmounted) return;
                    setChallengingUser(challenge.fromUser);
                    setOpenResponseModal(true);
                });
                
                connection.on('StartGame', callback => {
                    if(unmounted) return;
                    setRoomID(callback.roomID);
                });
                
                connection.on('MoveResponse', callback => {
                    if(unmounted) return;
                    setEnemyStats(callback.enemyShipStats);
                    setMyShipStats(callback.myShipStats);
                    setMyBoard(JSON.parse(callback.myBoard));
                    setEnemyBoard(JSON.parse(callback.enemyBoard));
                    setIsMyMove(callback.onMove);
                    setShowBoards(true);
                    setUpdate(true);
                })
            })
                .catch(e => {
                    if(e.statusCode === 401)
                    {
                        toast.error('Authentication failed.')
                        history.push('/');
                        localStorage.clear();
                    }
                }
            );
        }
        return () => 
        {
            if (connection) {
                connection.stop();
            }
            unmounted = true;
        }
    }, [connection]);
    

    const sendChallenge = async (user, aPlay) => {
        if(localStorage.getItem('login') === user) {
            toast.warn('You cannot send challenge to yourself.');
            return;
        }
        setAutoPlay(aPlay);

        const challenge = {
            toUser: user,
            fromUser: localStorage.getItem('login'),
        };

        if (connection) {
            if (connection.connectionStarted) {
                try {
                    await connection.send('SendChallenge', challenge);
                }
                catch(e) {
                    toast.error(e);
                }
        }}}

    const sendResponse = async (user, aPlay) => {
        const challenge = {
            toUser: challengingUser,
            fromUser: localStorage.getItem('login'),
        };
        setAutoPlay(aPlay);

        if (connection) {
            if (connection.connectionStarted) {
                try {
                    await connection.send('AcceptChallenge', challenge);
                }
                catch(e) {
                    toast.error(e);
                }
        }}}


    async function onSquareClick(x, y) {
        const move = {
            x: x,
            y: y,
            roomID: roomID,
            autoPlay: autoPlay
        }
        if (connection) {
            if (connection.connectionStarted) {
                try {
                    await connection.send('MakeMove', move);
                }
                catch(e) {
                    toast.error(e);
                }
        }}
    }



    return ( 
        <div className='game-panel-container'>
            <div className='game-panel-with-chat'>
                {showBoards && <div className='boards-container'>
                    <div className='board-container' >
                        <h1>Your board </h1>
                        <div className='board-with-stats'>
                            <Board isEnemy={false} isDisabled={isMyMove} data={myBoard}/>
                            <Stats data={myShipStats}/>
                        </div>
                    </div>
                    <div className='board-container'>
                        <h1>Enemy's board </h1>
                        <div className='board-with-stats'>
                            <Board isEnemy={true} isDisabled={!isMyMove} onSquareClick={onSquareClick} data={enemyBoard}/>
                            <Stats data={enemyStats}/>
                        </div>
                    </div>
                </div>}
                <Chat sendChallenge={sendChallenge} />
            </div>
            <ChallengeModal isOpen={openResponseModal} setOpen={setOpenResponseModal} 
                user={challengingUser} sendChallenge={sendResponse}
                isItResponse={true} />
        </div>
     );
}

export default GamePanel;