import React,{ useState } from "react";

/**
 * 
 * Cabecalho de separador
 * 
 * @param {*} text texto do cabecalho 
 * @param onClick gestor de clique no cabecalho
 * @returns 
 */
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