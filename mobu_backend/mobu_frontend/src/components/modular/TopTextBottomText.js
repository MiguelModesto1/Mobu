import React,{ useState } from "react";

/**
 * 
 * Conjunto de textos (superior e inferior)
 * 
 * @param {*} marginRight margem a direita
 * @param TTBTProps propriedades do conjunto textos : text.top, text.bottom 
 * @returns 
 */
export default function TopTextBottomText({isSelected= null, marginRight=null, TTBTProps}){ //
    
    return(
        <div 
        marginRight={marginRight === null ? undefined : marginRight}
        className="top-bottom-text-div">
            <span 
            style={isSelected ? undefined: {fontWeight: "bold"}} 
            className="top-text">
                {TTBTProps.top}
            </span>
            <br />
            <span className="bottom-text">
                {TTBTProps.bottom}
            </span>
        </div>
    );

    
}