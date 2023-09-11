import React,{ useState } from "react";
import Avatar from "./Avatar";

export default function PlayerItem({PIProps}){ 

    return(
        <div className="person-item">
            <Avatar avatarProps={PIProps.image}/>
            <span className="player-item-span">{PIProps.username}</span>
        </div>
    );

    
}