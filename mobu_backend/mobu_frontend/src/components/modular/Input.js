import React,{ useEffect, useRef, useState } from "react";

/**
 * 
 * Input generico
 * 
 * @param {*} input propriedades do input : title, type, value, placeholder
 * @param fromParent texto de classe do componente pai
 * @param onChange gestor de mudanca no input
 * @param display tipo de disposicao do input - in-line, bloco ou nenhum(none)
 * @param accept formato de ficheiros aceitaveis
 * @param name nome no servidor
 * @returns 
 */
export default function Input({ input, fromParent = null, onChange = null, display = null, accept = null, name = null}) {

    const span = useRef(<>
        <span className="input-title">{input.title}</span>
        <br />
    </>);

    //console.log(getText);

    return(
        <>
            {span.current}
            <input
                onChange={e => {
                    e.stopPropagation();
                    if(onChange !== null){
                        onChange(e.target.value);
                    }
                }}
                accept={accept === null ? undefined : accept}
                style={display === null ? undefined : {display: display}}
                className={fromParent === null ? undefined : fromParent + "-input"}
                name={ name === null ? undefined : name }
                type={input.type === "" ? "text" : input.type} 
                placeholder={input.placeholder === "" ? undefined : input.placeholder}
                value={input.value} />
        </ >
    );
}