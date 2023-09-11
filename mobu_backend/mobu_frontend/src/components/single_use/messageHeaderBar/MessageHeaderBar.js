import React,{ useState } from "react";
import TopTextBottomText from "../../modular/TopTextBottomText";
import ClickableIcon from "../../modular/ClickableIcon";
import "./MessageHeaderBar.css";

export default function MessageHeaderBar({text}){ 
    
    /* IMPLEMENTAR COM ESTADOS */

    return(
        <div className="message-header-bar">
            <TopTextBottomText TTBTProps={text}/>
            <ClickableIcon 
            CIProps={{
                size:"48px",
                fill:"none",
                path:{
                    d:"M12 18L24 30L36 18",
                    stroke:"white",
                    strokeWidth:"10",
                    strokeLineCap:"round",
                    strokeLinejoin:"round"
                }
            }}
            onClick={() => {alert("Implementar onclick de MessageHeaderBar chevron");}} />
        </div>
    );

    
}