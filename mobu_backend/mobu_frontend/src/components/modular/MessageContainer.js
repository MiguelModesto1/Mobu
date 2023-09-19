import React,{ useState } from "react";

/**
 * 
 * Contentor de mensagem generico
 * 
 * @param {*} children componentes filhos
 * @param fromParent texto de classe do pai
 * @returns 
 */
export default function MessageContainer({children, fromParent=null}){ 
    
    return(
        <div className={fromParent === null ? undefined : fromParent + "-message-container"}>
            {children}
        </div>
    );

    
}