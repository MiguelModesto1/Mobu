import React,{ useState } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";

export default function PersonGroupItem({PIProps, onClick}){ 
    
    return(
        <div
        onClick={e => {
            e.stopPropagation();
            onClick();
        }} 
        className="person-item">
            <Avatar avatarProps={PIProps.image}/>
            <TopTextBottomText TTBTProps={PIProps.text}/>
        </div>
    );

    
}