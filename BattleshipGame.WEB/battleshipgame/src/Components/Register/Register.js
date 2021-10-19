import React, {useState} from 'react';
import './Register.css';
import { toast } from "react-toastify";
import { useHistory } from "react-router-dom";
import axios from 'axios';


function Register() {
    let history = useHistory();
    const[login, setLogin] = useState('');
    const[password, setPassword] = useState('');
    const[confirmPassword, setConfirmPassword] = useState('');
    const[email, setEmail] = useState('');
    const URL = 'http://localhost:5000/api/Auth/';


    function onRegisterClick() {
        if(confirmPassword !== password) {
            toast.warning('Given password does not match.');
        }
        else {
            axios.post(URL + 'register',
            {
                login: login,
                password: password,
                email: email
            }).then(response => {
                toast.success('Successfully signed up!');
                history.push('/login');
            }).catch(error =>{
                if(typeof(error?.response?.data) !== 'undefined')
                    toast.error(error.response.data);
                toast.error(error.message);
            })
        }
    }

    function onBackToLogin() {
        history.push('/login');
    }

    function handleLoginChange(e) {
        e.preventDefault();
        setLogin(e.target.value);
    }

    function handlePasswordChange(e) {
        e.preventDefault();
        setPassword(e.target.value);
    }

    function handleConfirmPasswordChange(e) {
        e.preventDefault();
        setConfirmPassword(e.target.value);
    }

    function handleEmailChange(e) {
        e.preventDefault();
        setEmail(e.target.value);
    }


    return ( 
        <div className='register-container'>
            <h1> Sign Up! </h1>
            <div className='elements-register'>
                <input className='input' type="text" name="Login" placeholder="Login" onChange={handleLoginChange} value={login}/>
                <input className='input' type="text" name="Email" placeholder="Email" onChange={handleEmailChange} value={email}/>
                <input className='input' type="password" name="Password" placeholder="Password" onChange={handlePasswordChange} value={password}/>
                <input className='input' type="password" name="ConfirmPassword" placeholder="Confirm password" onChange={handleConfirmPasswordChange} value={confirmPassword}/>
                <button onClick={() => onRegisterClick()}>Register</button>
            </div>
            <label className='create-account-text' onClick={() => onBackToLogin()}>Back to login page</label>

        </div>
     );
}

export default Register;