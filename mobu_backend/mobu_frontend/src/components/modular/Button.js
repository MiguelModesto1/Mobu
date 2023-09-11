import React,{ useState } from "react";

export default function Button({text, color=null, fromParent=null, onClick}){ //
    
    return(
            <button
            style={color === null ? undefined :{background: color}}
            onClick={e => {
                e.stopPropagation();
                onClick();
            }} 
            className={fromParent === null ? undefined : fromParent + "-button"}>
                {text}
            </button>
    );

    
}