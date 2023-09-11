import React,{ useEffect, useMemo, useState } from "react";

export default function Input({input, fromParent=null,onChange=null, isPassword=null}) {

    const [getSpan, setSpan] = useState(<></>);
    const [getText, setText] = useState("");

    useEffect(() => {
        if(input.title == ""){
                setSpan(<></>);
            }else{
                setSpan(
                    <>
                        <span className="input-title">{input.title}</span>
                        <br />
                    </>
                );
            }
    }, []);

    //console.log(getText);

    return(
        <>
            {getSpan}
            <input
                onChange={e => {
                    e.stopPropagation();
                    setText(e.target.value);
                    if(onChange !== null){
                        onChange();
                    }
                }}
                className={fromParent === null ? undefined : fromParent + "-input"}
                type={input.type === "" ? "text" : input.type} 
                placeholder={input.placeholder === "" ? undefined : input.placeholder}
                value={getText} />
        </ >
    );
}