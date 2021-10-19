import React, { useEffect, useState } from 'react';
import './Chat.css'
import {GoPrimitiveDot} from 'react-icons/go'
import {IconContext} from "react-icons"
import ChallengeModal from './ChallengeModal';


function Lobby({loggedUsers, sendChallenge}) {

    const[openModal, setOpenModal] = useState(false);
    const[selectedUser, setSelectedUser] = useState('');
    const[users, setUsers] = useState([]);

    useEffect(() => {
        setUsers(loggedUsers);
    }, [loggedUsers])


    function userOnClick(u) {
        setOpenModal(true);
        setSelectedUser(u);
    }

    const mappedData = users.map(
        (u) => {
            return(
            <div key = {Math.random() * Date.now()}
                className='lobby-user'
                onClick={() =>userOnClick(u.username)}>
                <IconContext.Provider
                    value={{style: {fontSize: '20px',
                    color:"rgb(20, 138, 14)",
                    position: 'relative', top:'0px'}}}>         
                    <GoPrimitiveDot/>
                </IconContext.Provider>
                {u.username}
                {u.inGame?" <in-game>":""}
            </div>
                )}
    );

    return ( 
        <div className='lobby-container'>
            <h2>Lobby</h2>
            {mappedData}
            <ChallengeModal isOpen={openModal} setOpen={setOpenModal} 
                user={selectedUser} sendChallenge={sendChallenge}
                isItResponse={false}/>
        </div>
     );
}

export default Lobby
