import React,{ useState } from "react";

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