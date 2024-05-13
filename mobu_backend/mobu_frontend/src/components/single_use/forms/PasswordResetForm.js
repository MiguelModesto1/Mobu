import React,{ useState } from "react";
import Button from "../../modular/Button";
import Input from "../../modular/Input";

/**
 * Formulario de mudanca de password
 * @returns 
 */
export default function PasswordResetForm() { 

    const email = new URLSearchParams(window.location.search).get('email');

    const [currentPassword, setCurrentPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [passwordVerf, setPasswordVerf] = useState("");
    const [warningText, setWarningText] = useState("");
    
    /**
     * mudanca no estado da password atual
     */
    function handleCurrentPasswordChange(value){
        setCurrentPassword(value);
    }

    /**
     * mudanca no estado da nova password
     */
    function handleNewPasswordChange(value){
        setNewPassword(value);
    }

    /**
     * mudanca no estado da repeticao da nova password
     */
    function handlePasswordVerfChange(value){
        setPasswordVerf(value);
    }

    /**
     * clique no botao de submissao
     */
    async function handleButtonClick(){

        var options={
            method: 'POST',
            redirect: 'follow',
            body: JSON.stringify({
                NewPassword: newPassword,
                CurrentPassword: currentPassword,
                Email: email
            }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            }
        }

        if(newPassword === passwordVerf){
            await fetch(process.env.REACT_APP_API_URL + "/forgot-password/reset-password", options)
            .then((response) => {
                if (response.status === 200) {
                    window.location.assign("/")
                }else{
                    setWarningText("Tentativa de reiniciar password inválida");
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
                    <span className="warning-span" style={{ color: "#ff5f4a" }}>{warningText}</span>
            </div>
            :
            <></>}
            <div className="form-input-div">
                <span>Palavra-passe atual</span>
                <br />
                <input
                    type="password"
                    value={currentPassword}
                    className="form-input"
                    onChange={e => handleCurrentPasswordChange(e.target.value)}
                />
            </div>

            <div className="form-input-div">
                <span>Nova palavra-passe</span>
                <br />
                <input
                    type="password"
                    value={newPassword}
                    className="form-input"
                    onChange={e => handleNewPasswordChange(e.target.value)}
                />
            </div>

            <div className="form-input-div">
                <span>Repetir nova palavra-passe</span>
                <br />
                <input
                    type="password"
                    value={passwordVerf}
                    className="form-input"
                    onChange={e => handlePasswordVerfChange(e.target.value)}
                />
            </div>

            <button className="form-button" onClick={() => handleButtonClick()}>Mudar palavra-passe</button>
        </div>
    );

    
}