import React from 'react';
import './Chat.css'

function Message({user, date, text}) {
    return ( 
        <div className='message-container'>
            <div className='header'>
                <strong className='user-header'>{user} </strong>
                <label className='date-header'>{date}</label>
            </div>
            <p>{text}</p>
        </div>
     );
}

export default Message;