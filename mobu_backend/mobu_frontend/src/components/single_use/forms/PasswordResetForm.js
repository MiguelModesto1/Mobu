import React,{ useState } from "react";
import Button from "../../modular/Button";
import Input from "../../modular/Input";

/**
 * Formulario de mudanca de password
 * @returns 
 */
export default function PasswordResetForm(){ 

    const [currentPassword, setCurrentPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [passwordVerf, setPasswordVerf] = useState("");
    const[warningText, setWarningText] = useState("");
    
    function handleCurrentPasswordChange(value){
        setCurrentPassword(value);
    }

    function handlePasswordChange(value){
        setNewPassword(value);
    }

    function handlePasswordVerfChange(value){
        setPasswordVerf(value);
    }

    async function handleButtonClick(){

        var options={
            method: 'POST',
            redirect: 'follow',
            body: JSON.stringify({
                newPassword: newPassword,
                currentPassword: currentPassword
            }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            }
        }

        if(newPassword === passwordVerf){
            await fetch(process.env.REACT_APP_API_URL + "forgot-password/reset-password", options)
            .then((response) => response.status)
            .then((status) => {
                setWarningText(status === 404 ? "Tentativa de reiniciar password inválida" : "");
                if(status === 204){
                    //redirecionar aqui para a rota da pagina de mensagens
                }
            })
            .catch(err => {console.error("error", err)});
        }else{
            setWarningText("Passwords têm que conicidir!");
        }
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
                    type:"password",
                    title:"Palavra-passe atual",
                    value:"",
                    placeholder:""
                }}
                fromParent="form"
                onChange={handleCurrentPasswordChange}/>
                <Input input={{
                    type:"password",
                    title:"Nova palavra-passe",
                    value:"",
                    placeholder:""
                }}
                fromParent="form"
                onChange={handlePasswordChange}/>
                <Input input={{
                    type:"password",
                    title:"Repetir nova palavra-passe",
                    value:"",
                    placeholder:""
                }}
                fromParent="form"
                onChange={handlePasswordVerfChange}/>
            </div>
            <Button
            text="Mudar palavra-passe"
            fromParent="form"
            onClick={handleButtonClick} />
        </div>
    );

    
}