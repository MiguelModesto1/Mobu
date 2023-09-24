import React,{ useState, useEffect, useRef } from "react";
import { BrowserRouter as Router,Routes, Route, Link } from 'react-router-dom';
import Error404Component from "../single_use/Error404Component/Error404Component"

/**
 * 
 * Pagina de mosaico de jogo
 * 
 * @returns 
 */
export default function Error404Page(){
    return(
        <div className="error-404-page-div">
            <Error404Component />
        </div>
    );
}