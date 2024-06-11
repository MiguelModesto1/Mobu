import React,{ useState } from "react";

/**
 * 
 * Conjunto de textos (superior e inferior)
 * 
 * @returns 
 */
export default function TopTextBottomText({itemId, selectedItem, TTBTProps, fromParent=""}){ //

    /**
     * https://www.geeksforgeeks.org/how-to-truncate-a-string-in-javascript/
     * truncar texto
     * @param {any} text
     * @param {any} maxLength
     * @returns
     */
    const handleTextLength = (text, maxLength) => {

        for (var i = 0; i < text.length; i++) {
            if (text[i] === "\n" || i === maxLength) {
                return text.slice(0, i) + "...";
            }
        }

        return text;
    }


    return (
        <div className="">
            <span
                style={{ fontWeight: selectedItem === itemId ? "bold" : "normal" }} 
                className={fromParent + "-top-text"}>
                {TTBTProps.top}
            </span>
            <br />
            <span className={fromParent + "-bottom-text"}>
                {handleTextLength(TTBTProps.bottom, 20)}
            </span>
        </div>
    );

    
}