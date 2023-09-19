import React,{ useState } from "react";

/**
 * 
 * Item clicavel generico
 * 
 * @param {*} CIProps propriedades do icon : size, fill, path.d, path.stroke, path.strokeLinecap, path.strokeLinejoin
 * @param {*} fromParent
 * @param {*} onClick 
 * @returns 
 */
export default function ClickableIcon({CIProps, fromParent=null, onClick}){ 
    
    return(
        <div 
        onClick={e => {
            e.stopPropagation();
            onClick();
        }} 
        className={fromParent === null ? undefined : fromParent + "-clickable-icon"}
        width={CIProps.size} 
        height={CIProps.size} 
        >
            <svg 
            xmlns="http://www.w3.org/2000/svg"
            width={CIProps.size} 
            height={CIProps.size} 
            viewBox={"0 0 " + CIProps.size + " " + CIProps.size}
            fill={CIProps.fill}>
                <path 
                d={CIProps.path.d} 
                stroke={CIProps.path.stroke}
                strokeWidth={CIProps.path.strokeWidth}
                strokeLinecap={CIProps.path.strokeLinecap}
                strokeLinejoin={CIProps.path.strokeLinejoin}/>
            </ svg>
        </div>
    );

    
}