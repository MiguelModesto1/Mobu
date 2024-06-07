import React,{ useState } from "react";

/**
 * 
 * Conjunto de textos (superior e inferior)
 * 
 * @returns 
 */
export default function TopTextBottomText({itemId, isSelectedItem, marginRight=null, TTBTProps, fromParent=""}){ //
    
    return(
        <div 
        //marginRight={marginRight === null ? undefined : marginRight}
        className={fromParent + "-top-bottom-text-div"}>
            <span
                style={{ fontWeight: isSelectedItem === itemId ? "bold" : "normal" }} 
                className={fromParent + "-top-text"}>
                {TTBTProps.top}
            </span>
            <br />
            <span className={fromParent + "-bottom-text"}>
                {TTBTProps.bottom}
            </span>
        </div>
    );

    
}