import React,{ useState } from "react";

/**
 * 
 * Item de menu
 * 
 * @param {*} text
 * @param onClick gestor de clique no item
 * @param onClickPrm parametro para passar ao gestor 
 * @returns 
 */
export default function MenuItem({text, onClick, onClickPrm}){

    return(
        <div onClick={ e => {
            e.stopPropagation();
            onClick(onClickPrm);
        }} 
        className="menu-item">
            {text}
        </div>
    );

}