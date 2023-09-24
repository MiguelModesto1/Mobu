import React,{ useEffect, useState } from "react";

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

    const [span, setSpan] = useState(<></>);
    const [text, setText] = useState("");

    useEffect(() => {
        if(input.title === ""){
                setSpan(<></>);
            }else{
                setSpan(
                    <>
                        <span className="input-title">{input.title}</span>
                        <br />
                    </>
                );
            }
    }, [input.title]);

    //console.log(getText);

    return(
        <>
            {span}
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