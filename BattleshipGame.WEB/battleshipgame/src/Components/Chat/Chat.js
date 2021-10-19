import React, {useRef, useEffect, useState} from 'react';
import './Chat.css'
import Lobby from './Lobby';
import Message from './Message';
import { HubConnectionBuilder } from '@microsoft/signalr';
import {useHistory} from 'react-router-dom'
import { toast } from 'react-toastify';



function Chat() {
    let history = useHistory();
    const messagesEndRef = useRef(null);
    const [connection, setConnection] = useState(null);
    const [chat, setChat] = useState([]);
    const [messageInput, setMessageInput] = useState('');

    const scrollToBottom = () => {
      messagesEndRef.current.scrollIntoView({ behavior: "smooth" })
    }

    function handleMessageInput(e) {
        e.preventDefault();
        setMessageInput(e.target.value);
    }

    useEffect(scrollToBottom, []); 

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/hub/chat', { accessTokenFactory: () => localStorage.getItem('token') })
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }, []);

    useEffect(() => {
        if (connection) {
            connection.start()
            .then(result => {    
                connection.on('ReceiveMessage', message => {
                        console.log(chat);
                        const newChat = chat;
                        newChat.push(message);
                        setChat(newChat);
                    });
                })
                .catch(e => {
                    if(e.statusCode === 401)
                    {
                        toast.error('Authentication problem.')
                        history.push('/');
                        localStorage.clear();
                    }
                }
            );
        }
    }, [connection]);

    const sendMessage = async (user, message) => {
        const chatMessage = {
            user: user,
            text: message,
            date: new Date().toLocaleString()
        };
        if (connection) {
            if (connection.connectionStarted) {
                try {
                    await connection.send('SendMessage', chatMessage);
                }
                catch(e) {
                    toast.error(e);
                }
            }
        }
        else {
            toast.error("No connection to server.");
        }
    }

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

    const mappedMessages = chat.map(
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
                    <input className='chat-input-input' placeholder='Write a message...' value={messageInput} onChange={handleMessageInput}></input>
                    <button className='chat-button' 
                        onClick={() => sendMessage(localStorage.getItem('login'), messageInput)}>
                            Send</button>
                </div>
            </div>
            <Lobby/>
        </div>
     );
}

export default Chat;