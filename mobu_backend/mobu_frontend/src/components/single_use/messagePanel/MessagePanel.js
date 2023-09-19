import React,{ useState } from "react";
import "./MessagePanel.css"

/**
 * 
 * Painel de mensagens
 * 
 * @param {*} children filhos
 * @returns 
 */
export default function MessagePanel({children}){ 
    
    /* IMPLEMENTAR COM ESTADOS */

    return(
        <div className="message-panel">
            {children}
        </div>
    );

    
}