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
export default function Button({text, color=null, fromParent=null, onClick, params=null}){ //
    
    return(
            <button
            style={color === null ? undefined :{background: color}}
            onClick={e => {
                e.stopPropagation();
                onClick(params === null ? undefined : params[0], params[1]);
            }} 
            className={fromParent === null ? undefined : fromParent + "-button"}>
                {text}
            </button>
    );

    
}