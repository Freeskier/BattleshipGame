import React, { useEffect, useState } from 'react';
import './Modal.css';
import Modal from 'react-modal';

Modal.setAppElement('#root');

function ChallengeModal({isOpen, setOpen, user}) {

    const [remainingTime, setReminingTime] = useState(5);

    function toggleModal() {
        setOpen(false);
    }

    useEffect(() => {
        const interval = setInterval(() => {
            setReminingTime((remainingTime) => remainingTime - 1);

            if(!isOpen)
                clearInterval(interval)
 
        }, 1000);
        return () => 
        {
            clearInterval(interval);
            setReminingTime(5);
        }
    },[isOpen])

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
                    <h3>Do you want to challenge {user}?</h3>
                    <p>Remaining time: {remainingTime}s...</p>
                </div>
                <button onClick={toggleModal}>Close</button>
            </Modal>
        </div>
     );
}

export default ChallengeModal;