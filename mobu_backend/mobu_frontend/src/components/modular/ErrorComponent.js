import React from "react";

/**
 * 
 * Componente de erro
 * 
 * @returns 
 */
export default function ErrorComponent({error, text}){
    
    return(
        <div style={{textAlign:"center"}} className="error-div">
            <span className="error-span">{error} <br />{text}</span>
        </div>
    );
}