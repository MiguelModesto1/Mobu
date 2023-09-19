import React,{ useState } from "react";

/**
 * 
 * Botao generico
 * 
 * @param {*} text texto do botao
 * @param {*} color cor do botao
 * @param {*} fromParent texto de classe do componente pai
 * @param {*} onClick gestor de clique do botao
 * @returns 
 */
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