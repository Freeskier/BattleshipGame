import React from 'react';
import './Login.css'


function Login() {

    function onLoginClick() {

    }

    function onCreateAccountClick() {
    }

    return ( 
        <div className='login-container'>
            <h1> Sign In! </h1>
            <div className='elements-login'>
                <input type="text" name="Login" placeholder="Login" />
                <input type="password" name="Password" placeholder="Password" />
                <button onClick={() => onLoginClick()}>Sign in</button>
            </div>
            <label className='create-account-text' onClick={() => onCreateAccountClick()}>Create account</label>
        </div>
     );
}

export default Login;