import React from 'react';
import './Header.css'
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import {FiLogOut} from 'react-icons/fi';
import {IconContext} from "react-icons"
import { toast } from "react-toastify";
import {useHistory} from 'react-router-dom'

function Header({username, setUsername}) {
    let history = useHistory();

    function onLogout(){
        history.push('/login');
        setUsername('');
        localStorage.clear();
        toast.info('See you :)');
    }

    return ( 
        <div className='container'>
            <div className='header-container'>
                <label className='header-title'>Battleship HappyGame</label>
                {username !== ""?
                <div className='profile' onClick={() => onLogout()}>
                    <IconContext.Provider value={{style: {fontSize: '25px', color: "rgb(255, 255, 255)", position: 'relative', top:'4px'}}}> 
                        <FiLogOut />
                    </IconContext.Provider>
                    <label className='profile-text'>{username}</label>
                </div>
                :<div/>}
            </div>
            <ToastContainer
                autoClose={2000}
                />
        </div>
     );
}

export default Header;