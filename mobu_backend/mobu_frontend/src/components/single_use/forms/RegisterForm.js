import React,{ useState } from "react";
import Link from "../../modular/Link";
import Button from "../../modular/Button";
import Input from "../../modular/Input";

export default function RegisterForm(){ 
    
    function handleChange(){
        alert("Implementar handleChange de RegisterForm");
    }

    function handleButtonClick(){
        alert("Implementar handleButtonClick de RegisterForm");
    }

    return(
        <div className="form">
            <div className="form-input-div">
                <Input input={{
                    type:"text",
                    title:"Nome de utilizador",
                    value:"",
                    placeholder:""
                }}
                fromParent="form"
                onChange={handleChange}/>
            </div>

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
            
            <div className="form-input-div">
                <Input input={{
                type:"password",
                title:"Repetir palavra-passe",
                value:"",
                placeholder:""
                }}
                fromParent="form"
                onChange={handleChange}/>
            </div>
            
            <Button
            text="Registar"
            fromParent="form"
            onClick={handleButtonClick} />
            <div className="links-div">
                <Link linkProps={{
                    href:"",
                    text:"Já tenho conta"
                }}/>
                <span> </span>
                <Link linkProps={{
                    href:"",
                    text:"Continuar como convidado"
                }}/>
            </div>
        </div>
    );
}