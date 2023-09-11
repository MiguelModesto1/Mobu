import React,{ useState } from "react";

export default function TabHeader({text, onClick}){ 
    
    return(
        <div onClick={ e => {
            e.stopPropagation();
            onClick();
        }} 
        className="tab-header">
            {text}
        </div>
    );

    
}