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
    const [enemyBoard, setnemyBoard] = useState([]);
    const [openResponseModal, setOpenResponseModal] = useState(false);
    const [challengingUser, setChallengingUser] = useState('');

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/hub/game', { accessTokenFactory: () => localStorage.getItem('token') })
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
            .then(result => {    
                connection.on('ChallengeUserCallback', challenge => {
                    setChallengingUser(challenge.fromUser);
                    setOpenResponseModal(true);
                    console.log("ASD");
                });

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
    }, [connection]);

    const sendChallenge = async (user) => {
        if(localStorage.getItem('login') === user) {
            toast.warn('You cannot send challenge to yourself.');
            return;
        }

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

    const sendResponse = async () => {
        const challenge = {
            toUser: challengingUser,
            fromUser: localStorage.getItem('login'),
        };

        if (connection) {
            if (connection.connectionStarted) {
                try {
                    await connection.send('AcceptChallenge', challenge);
                }
                catch(e) {
                    toast.error(e);
                }
        }}}

    return ( 
        <div className='game-panel-container'>
            <div className='game-panel-with-chat'>
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
                <Chat sendChallenge={sendChallenge} />
            </div>
            <ChallengeModal isOpen={openResponseModal} setOpen={setOpenResponseModal} 
                user={challengingUser} sendChallenge={sendResponse}
                isItResponse={true}/>
        </div>
     );
}

export default GamePanel;