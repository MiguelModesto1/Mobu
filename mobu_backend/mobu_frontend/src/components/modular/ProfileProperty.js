import React,{ useMemo, useEffect, useState } from "react";
import Input from "./Input";

/**
 * 
 * Propriedade de perfil
 * 
 * @param {*} keyProp chave da propriedade
 * @param text valor da propriedade
 * @param isEditing booleano de edicao
 * @param isChangingPassword booleano de mudanca de password 
 * @param onChangeText gestor de mudanca de texto
 * @returns 
 */
export default function ProfileProperty({keyProp, isEditing, isChangingPassword, onChangeText=null}){

    const [renderResult, setRenderResult] = useState(<></>);
    const [text, setText] = useState("");

    useMemo(() => {
        
        const handleTextChange = (value) => {
            setText(value);
            if(onChangeText !== null){
                onChangeText(value);
            }
        }
        
        if(isEditing){
            setRenderResult(<Input input={{
                title:"",
                type:"text",
                placeholder:"",
                value:text
            }}
            fromParent="profile" 
            onChange={handleTextChange}/>);
        }else if(isChangingPassword){
            setRenderResult(<Input input={{
                title:"",
                type:"password",
                placeholder:"",
                value:""
            }}
            fromParent="profile"
            onChange={handleTextChange} />);
        }else{
            setRenderResult(
                <span className="profile-value-span">{text}</span>
            );
        }

        

    }, [isEditing, isChangingPassword, text, onChangeText])
    
    return(
        <div className="profile-prop-div">
            <span className="profile-key-span">{keyProp + " :"}</span>
            {renderResult}
        </div>
    );

}