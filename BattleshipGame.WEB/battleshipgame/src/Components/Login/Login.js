import React, { useState } from 'react';
import axios from 'axios';
import './Login.css'
import { toast } from "react-toastify";
import { useHistory } from "react-router-dom";


function Login({setName}) {
    let history = useHistory();
    const[login, setLogin] = useState('');
    const[password, setPassword] = useState('');
    const URL = 'https://localhost:5001/api/Auth/';


    function onLoginClick() {
        axios.post(URL + 'login',
        {
            login: login,
            password: password
        }
        ).then(response => {
            toast.success('Successfully logged in!');
            localStorage.setItem('token', response.data.token);
            localStorage.setItem('login', response.data.login);
            setName(response.data.login);
            history.push('/game');
        }).catch(error =>{
            if(typeof(error?.response?.data) !== 'undefined')
                toast.error(error.response.data);
            toast.error(error.message);
        })
    }

    function onCreateAccountClick() {
        history.push('/register');
    }

    function handleLoginChange(e) {
        e.preventDefault();
        setLogin(e.target.value);
    }

    function handlePasswordChange(e) {
        e.preventDefault();
        setPassword(e.target.value);
    }

    return ( 
        <div className='login-container'>
            <h1> Sign In! </h1>
            <div className='elements-login'>
                <input className='input'  type="text" name="Login" placeholder="Login" onChange={handleLoginChange} value={login}/>
                <input className='input'  type="password" name="Password" placeholder="Password" onChange={handlePasswordChange} value={password}/>
                <button onClick={() => onLoginClick()}>Sign in</button>
            </div>
            <label className='create-account-text' onClick={() => onCreateAccountClick()}>Create account</label>
        </div>
     );
}

export default Login;