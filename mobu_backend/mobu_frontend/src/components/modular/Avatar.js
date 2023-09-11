import React,{ useState } from "react";

export default function Avatar({avatarProps}){ //
    
    return(
        <div 
        className="avatar-div" 
        width={avatarProps.size} 
        height={avatarProps.size}>
            <img className="avatar"
            src={avatarProps.src === "" ? undefined : avatarProps.src}
            alt={avatarProps.alt}
            height={avatarProps.size} />
        </div>
    );

    
}