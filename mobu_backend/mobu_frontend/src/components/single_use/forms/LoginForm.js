import React,{ useState } from "react";
import Button from "../../modular/Button";
import Link from "../../modular/Link";
import Input from "../../modular/Input";

export default function LoginForm(){ 
    
    function handleChange(){
        alert("Implementar handleChange de LoginForm");
    }

    function handleButtonClick(){
        alert("Implementar handleButtonClick de LoginForm");
    }

    return(
        <div className="form">
            <div className="form-input-div">
                <Input input={{
                type:"text",
                title:"ID",
                value:"",
                placeholder:""
                }}
                fromParent="form"
                onChange={handleChange}/>
            </div>
            
            <div className="form-input-div">
                <Input input={{
                    type:"password",
                    title:"Palavra-passe",
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
            <div className="links-div">
                <Link linkProps={{
                    href:"",
                    text:"Esqueci-me da palavra-passe"
                }}/>
                <Link linkProps={{
                    href:"",
                    text:"Não tenho uma conta"
                }}/>
            </div>
        </div>
    );

    
}