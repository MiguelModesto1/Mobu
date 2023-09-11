import React,{ useState } from "react";
import Button from "../../modular/Button";
import Input from "../../modular/Input";

export default function ForgotPasswordForm(){ 
    
    function handleChange(){
        alert("Implementar handleChange de ForgotPasswordForm");
    }

    function handleButtonClick(){
        alert("Implementar handleButtonClick de ForgotPasswordForm");
    }

    return(
        <div className="form">
            <div className="form-input-div">
                <Input input={{
                    type:"email",
                    title:"E-mail",
                    value:"",
                    placeholder:""
                }}
                fromParent="form"
                onChange={handleChange}/>
            </div>
            <Button
            text="Log in"
            fromParent="form"
            onClick={handleButtonClick} />
        </div>
    );

    
}