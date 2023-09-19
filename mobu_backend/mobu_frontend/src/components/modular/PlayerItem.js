import React,{ useState } from "react";
import Avatar from "./Avatar";

/**
 * 
 * Item de jogador (mosaicos flutuantes que aparecem durante um jogo)
 * 
 * @param {*} PIProps propriedades do item : image, username 
 * @returns 
 */
export default function PlayerItem({PIProps}){ 

    return(
        <div className="person-item">
            <Avatar avatarProps={PIProps.image}/>
            <span className="player-item-span">{PIProps.username}</span>
        </div>
    );

    
}