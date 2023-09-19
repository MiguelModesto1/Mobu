import React from "react";

/**
 * 
 * Link generico
 * 
 * @param {*} linkProps propriedades do link : href, text
 * @param {*} fromParent texto de classe do componente pai
 * @param onClick gestor de clique no link 
 * @returns 
 */
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