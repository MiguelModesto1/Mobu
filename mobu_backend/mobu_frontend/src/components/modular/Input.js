import React,{ useEffect, useRef, useState } from "react";

/**
 * 
 * Input generico
 * 
 * @param {*} input propriedades do input : title, type, value, placeholder
 * @param fromParent texto de classe do componente pai
 * @param onChange gestor de mudanca no input
 * @returns 
 */
export default function Input({input, fromParent=null,onChange=null, display=null}) {

    const [text, setText] = useState("");

    const span = useRef(<></>);

    useEffect(() => {
        if (input.title === "") {
            span.current = <></>;
        } else {
            span.current = 
                    <>
                        <span className="input-title">{input.title}</span>
                        <br />
                    </>
                ;
            }
    }, [input.title]);

    //console.log(getText);

    return(
        <>
            {span.current}
            <input
                onChange={e => {
                    e.stopPropagation();
                    setText(e.target.value);
                    if(onChange !== null){
                        onChange(e.target.value);
                    }
                }}
                style={display === null ? undefined : {display: display}}
                className={fromParent === null ? undefined : fromParent + "-input"}
                type={input.type === "" ? "text" : input.type} 
                placeholder={input.placeholder === "" ? undefined : input.placeholder}
                value={text} />
        </ >
    );
}