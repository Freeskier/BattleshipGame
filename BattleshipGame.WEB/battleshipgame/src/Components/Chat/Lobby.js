import React, { useState } from 'react';
import './Chat.css'
import {GoPrimitiveDot} from 'react-icons/go'
import {IconContext} from "react-icons"
import ChallengeModal from './ChallengeModal';


function Lobby() {

    const[openModal, setOpenModal] = useState(false);
    const[selectedUser, setSelectedUser] = useState('');

    const exampleData = [
        {username: "Kotek", inGame: true},
        {username: "Piesek", inGame: false},
        {username: "Królik", inGame: false},
    ];

    function userOnClick(u) {
        setOpenModal(true);
        setSelectedUser(u);
    }

    const mappedData = exampleData.map(
        (u) => {
            return(
            <div key = {Math.random() * Date.now()}
                className='lobby-user'
                onClick={() =>userOnClick(u.username)}>
                <IconContext.Provider
                    value={{style: {fontSize: '20px',
                    color:"rgb(55, 255, 50)",
                    position: 'relative', top:'0px'}}}>         
                    <GoPrimitiveDot/>
                </IconContext.Provider>
                {u.username}
                {u.inGame?" <in-game>":""}
            </div>
                )}
    )

    return ( 
        <div className='lobby-container'>
            <h2>Lobby</h2>
            {mappedData}
            <ChallengeModal isOpen={openModal} setOpen={setOpenModal} user={selectedUser} />
        </div>
     );
}

export default Lobby
