import React from 'react';
import './Chat.css'
import Lobby from './Lobby';
import Message from './Message';

function Chat() {

    const messages =[
        {user:'Kazik', date: '21-03-2323', text: "asdsadas"},
        {user:'Julek', date: '21-03-2323', text: "asdxzc"},
        {user:'Sylwia', date:'21-03-2323', text: "xczczxczxczxczx"},
    ];

    const mappedMessages = messages.map(
        (val) => <Message 
            key = {Math.random() * Date.now()}
            text={val.text}
            date={val.date}
            user={val.user}/>
    )

    return ( 
        <div className='chat-and-lobby'>
            <div className='chat-container'>
                <h2>Chat</h2>
                <div className='messages-container'>
                    {mappedMessages}
                </div>
                <input placeholder='Write a message...'></input>
            </div>
            <Lobby/>
        </div>
     );
}

export default Chat;