import React,{ useMemo, useEffect, useState } from "react";
import Input from "./Input";

export default function ProfileProperty({keyProp, text, isEditing, isChangingPassword}){

    const [getRenderResult, setRenderResult] = useState(<></>);

    useMemo(() => {
        if(isEditing){
            setRenderResult(<Input input={{
                title:"",
                type:"text",
                placeholder:"",
                value:text
            }}
            fromParent="profile" />);
        }else if(isChangingPassword){
            setRenderResult(<Input input={{
                title:"",
                type:"password",
                placeholder:"",
                value:""
            }}
            fromParent="profile" />);
        }else{
            setRenderResult(
                <span className="profile-value-span">{text}</span>
            );
        }

    }, [setRenderResult, isChangingPassword, isEditing])
    
    return(
        <div className="profile-prop-div">
            <span className="profile-key-span">{keyProp + " :"}</span>
            {getRenderResult}
        </div>
    );

}