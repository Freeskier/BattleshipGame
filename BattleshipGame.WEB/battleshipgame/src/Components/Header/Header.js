import React from 'react';
import './Header.css'
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

function Header() {
    return ( 
        <div className='container'>
            <div className='header-container'>
                <label className='header-title'>Battleship HappyGame</label>
            </div>
            <ToastContainer
                autoClose={2000}
                />
        </div>
     );
}

export default Header;