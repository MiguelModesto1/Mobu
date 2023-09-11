import React,{ useState } from "react";
import "./MessagePanel.css"

export default function MessagePanel({children}){ 
    
    /* IMPLEMENTAR COM ESTADOS */

    return(
        <div className="message-panel">
            {children}
        </div>
    );

    
}