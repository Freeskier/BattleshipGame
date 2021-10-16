import React from 'react';
import './Register.css';
import { toast } from "react-toastify";


function Register() {

    function onRegisterClick() {
        toast.success('Succesfully signed up!')
        
    }


    return ( 
        <div>

        <div className='register-container'>
            <h1> Sign Up! </h1>
            <div className='elements-register'>
                <input type="text" name="Login" placeholder="Login" />
                <input type="text" name="Email" placeholder="Email" />
                <input type="password" name="Password" placeholder="Password" />
                <input type="password" name="ConfirmPassword" placeholder="Confirm password" />
                <button onClick={() => onRegisterClick()}>Register</button>
            </div>
        </div>
        </div>
     );
}

export default Register;