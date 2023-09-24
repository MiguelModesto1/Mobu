import React,{ useState, useEffect, useRef } from "react";
import { useLocation } from 'react-router-dom';
import GameTile from "../modular/GameTile"

/**
 * 
 * Pagina de mosaico de jogo
 * 
 * @returns 
 */
export default function GameTilePage(){
    return(
        <div className="game-tile-page-div">
            <GameTile />
        </div>
    );
}