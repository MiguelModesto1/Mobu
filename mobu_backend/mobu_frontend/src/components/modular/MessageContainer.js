import React,{ useState } from "react";

export default function MessageContainer({children, fromParent=null}){ 
    
    return(
        <div className={fromParent === null ? undefined : fromParent + "-message-container"}>
            {children}
        </div>
    );

    
}