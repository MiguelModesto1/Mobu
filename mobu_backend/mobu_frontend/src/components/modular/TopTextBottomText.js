import React,{ useState } from "react";

export default function TopTextBottomText({marginRight=null, TTBTProps}){ //
    
    return(
        <div 
        marginRight={marginRight === null ? undefined : marginRight}
        className="top-bottom-text-div">
            <span className="top-text">
                {TTBTProps.top}
            </span>
            <br />
            <span className="bottom-text">
                {TTBTProps.bottom}
            </span>
        </div>
    );

    
}