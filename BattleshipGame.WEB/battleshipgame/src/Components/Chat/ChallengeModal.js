import React, { useEffect, useState } from 'react';
import './Modal.css';
import Modal from 'react-modal';
import {FaCheck, FaTimes} from 'react-icons/fa';
import {IconContext} from "react-icons"


Modal.setAppElement('#root');

function ChallengeModal({isOpen, setOpen, user, sendChallenge, isItResponse}) {

    const [remainingTime, setReminingTime] = useState(1000);
    const [aPlay, setAPlay] = useState(false);

    function toggleModal() {
        setOpen(false);
    }

    function accept() {
        sendChallenge(user, aPlay);
        setOpen(false);
    }

    useEffect(() => {
        if(isOpen)
            setReminingTime(1000);  
    },[isOpen])

    useEffect(() => {
        let interval;
        interval = setInterval(() => {
            setReminingTime(remainingTime => remainingTime - 1);
        }, 1000)
        if(remainingTime <= 0 && isOpen) {
            clearInterval(interval);
            setOpen(false);
        }
        return () => {
            clearInterval(interval);
        }
    },[isOpen, remainingTime, setOpen]);



    return ( 
        <div>
            <Modal
                isOpen={isOpen}
                onRequestClose={toggleModal}
                contentLabel="Challenge"
                className='challenge-modal'
                overlayClassName='overlay-challenge-modal'
                closeTimeoutMS={500}
                >
                <div>
                    {!isItResponse?
                        <h3>Do you want to challenge {user}?</h3>
                    :
                        <h3>{user} invites you to play/</h3>}
                    <p>Remaining time: {remainingTime}s...</p>
                </div>
                <div className='checkbox'> 
                    <input id='checkbox' type="checkbox" defaultChecked={aPlay} onChange={() => setAPlay(aPlay => !aPlay)}/>
                    <label htmlFor='checkbox'>Auto play</label>
                </div>
                <IconContext.Provider value={{style: {fontSize: '25px', color: "rgb(255, 255, 255)", position: 'relative', top:'4px'}}}> 
                    <button className='red-button' onClick={toggleModal}><FaTimes/></button>
                    <button className='green-button' onClick={() => accept()}><FaCheck/></button>
                </IconContext.Provider>
            </Modal>
        </div>
     );
}

export default ChallengeModal;