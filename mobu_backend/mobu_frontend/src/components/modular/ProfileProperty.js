import React,{ useRef, useEffect, useState } from "react";
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

    const renderResult = useRef(<></>);
    const [text, setText] = useState("");

    useEffect(() => {
        
        const handleTextChange = (value) => {
            setText(value);
            if(onChangeText !== null){
                onChangeText(value);
            }
        }
        
        if (isEditing) {
            renderResult.current = <Input input={{
                title: "",
                type: "text",
                placeholder: "",
                value: text
            }}
                fromParent="profile"
                onChange={handleTextChange} />;
        } else if (isChangingPassword) {
            renderResult.current = <Input input={{
                title:"",
                type:"password",
                placeholder:"",
                value:""
            }}
            fromParent="profile"
            onChange={handleTextChange} />;
        } else {
            renderResult.current = 
                <span className="profile-value-span">{text}</span>
            ;
        }

        

    }, [isEditing, isChangingPassword, text, onChangeText])
    
    return(
        <div className="profile-prop-div">
            <span className="profile-key-span">{keyProp + " :"}</span>
            {renderResult.current}
        </div>
    );

}