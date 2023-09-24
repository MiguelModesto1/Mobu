import React,{ useState, useEffect, useRef } from "react";
import { useLocation } from 'react-router-dom';
import ThanksComponent from "../single_use/ThanksComponent/ThanksComponent"

/**
 * 
 * Pagina de mosaico de jogo
 * 
 * @returns 
 */
export default function ThanksPage(){
    return(
        <div className="thanks-page-div">
            <ThanksComponent />
        </div>
    );
}