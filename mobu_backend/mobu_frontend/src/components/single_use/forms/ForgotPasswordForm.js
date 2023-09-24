import React,{ useState } from "react";
import Button from "../../modular/Button";
import Input from "../../modular/Input";

/**
 * Formulario de password esquecida
 * @returns 
 */
export default function ForgotPasswordForm(){ 

    const[email, setEmail] = useState("");
    const[warningText, setWarningText] = useState("");
    
    function handleChange(value){
        setEmail(value);
    }

    async function handleButtonClick(){

        var options={
            method: 'POST',
            redirect: 'follow',
            body: JSON.stringify({
                email: email
            }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            }
        }

        await fetch(process.env.REACT_APP_API_URL + "/forgot-password/send-email", options)
        .then((response) => {
            if(response.status === 204){
                window.location.assign("./password-reset");
            }else{
                setWarningText("Tentativa de envio de email inválida");
            }
        })
        .catch(err => {console.error("error", err)});
    }

    return(
        <div className="form">
            {warningText !== "" ?
            <div className="warning-span-div">
                <span className="warning-span" color="#ff5f4a">{warningText}</span>
            </div>
            :
            <></>}
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
            text="Enviar email"
            fromParent="form"
            onClick={handleButtonClick} />
        </div>
    );

    
}