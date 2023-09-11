import React,{ useState } from "react";

export default function Link({linkProps, fromParent=null, onClick=null}){ 
    
    return(
            <a 
            onClick={onClick === null ? undefined :
                e => {
                    e.preventDefault();
                    e.stopPropagation();
                    onClick(); 
                }
            }
            className={fromParent === null ? undefined : fromParent + "-link"} 
            href={linkProps.href}>
                {linkProps.text}
            </a>
    );

    
}