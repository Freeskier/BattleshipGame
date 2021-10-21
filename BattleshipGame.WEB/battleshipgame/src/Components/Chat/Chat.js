import React, {useRef, useEffect, useState} from 'react';
import './Chat.css'
import Lobby from './Lobby';
import Message from './Message';
import { HubConnectionBuilder } from '@microsoft/signalr';
import {useHistory} from 'react-router-dom'
import { toast } from 'react-toastify';



function Chat({sendChallenge}) {
    let history = useHistory();
    const messagesEndRef = useRef(null);
    const [connection, setConnection] = useState(null);
    const [chat, setChat] = useState([]);
    const [loggedUsers, setLoggedUsers] = useState([]);
    const [messageInput, setMessageInput] = useState('');
    const latestChat = useRef(null);

    latestChat.current = chat;

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
            .withUrl('http://localhost:5000/hub/chat', { accessTokenFactory: () => localStorage.getItem('token') })
            .withAutomaticReconnect()
            .build();
        setConnection(newConnection);
    }, []);

    useEffect(() => {
        let unmounted = false;
        if (connection) {
            connection.start()
            .then(result => {    
                connection.on('ReceiveMessage', message => {
                        const newChat = [...latestChat.current];
                        newChat.push(message);
                        if(!unmounted) 
                            setChat(newChat);
                        scrollToBottom();
                    });

                connection.on('LoggedUsers', resp => {
                    if(!unmounted)
                        setLoggedUsers(resp);
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
        return () => 
        {
            if (connection) {
                connection.stop();
            }
            unmounted = true;
        }
    }, [connection, history]);

    const sendMessage = async (user, message) => {
        if(message === '') {
            toast.warn("Can't send empty message.");
            return;
        }
        const chatMessage = {
            user: user,
            text: message,
            date: new Date().toLocaleString()
        };
        if (connection) {
            if (connection.connectionStarted) {
                try {
                    await connection.send('SendMessage', chatMessage);
                    setMessageInput('');
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
            <Lobby loggedUsers={loggedUsers} sendChallenge={sendChallenge} />
        </div>
     );
}

export default Chat;