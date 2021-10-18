import React, {useRef, useEffect, useState} from 'react';
import './Chat.css'
import Lobby from './Lobby';
import Message from './Message';
import { HubConnectionBuilder } from '@microsoft/signalr';


function Chat() {
    const messagesEndRef = useRef(null);
    const [connection, setConnection] = useState(null);
    const [chat, setChat] = useState([]);

    const scrollToBottom = () => {
      messagesEndRef.current.scrollIntoView({ behavior: "smooth" })
    }

    useEffect(scrollToBottom, []); 

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/chatHub', { accessTokenFactory: () => "" })
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    console.log('Connected!');
    
                    connection.on('ReceiveMessage', message => {

                    });
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const messages =[
        {user:'Kazik', date: '21-03-2323', text: "asdsadas"},
        {user:'Julek', date: '21-03-2323', text: "asdxzc"},
        {user:'Sylwia', date:'21-03-2323', text: "xczczxczxczxczx"},
        {user:'Kazik', date: '21-03-2323', text: "asdsadas"},
        {user:'Julek', date: '21-03-2323', text: "asdxzc"},
        {user:'Sylwia', date:'21-03-2323', text: "xczczxczxczxczx"},
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
                    <div ref={messagesEndRef} />
                </div>
                <div className='chat-input'>
                    <input className='chat-input-input' placeholder='Write a message...'></input>
                    <button className='chat-button'>Send</button>
                </div>
            </div>
            <Lobby/>
        </div>
     );
}

export default Chat;