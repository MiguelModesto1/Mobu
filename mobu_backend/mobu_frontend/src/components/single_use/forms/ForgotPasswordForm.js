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

    /**
     * mudanca no estado do email
     */
    function handleEmailChange(value) {
        setEmail(value);
    }
    /**
     * clique no botao de submissao
     */
    async function handleButtonClick(){

        var options={
            method: 'POST',
            redirect: 'follow',
            body: JSON.stringify({
                Email: email
            }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            }
        }

        await fetch(process.env.REACT_APP_API_URL + "/forgot-password/send-email", options)
        .then((response) => {
            if(response.status !== 200){
                setWarningText("Tentativa de envio de email inválida");
            }
        })
        .catch(err => {console.error("error", err)});
    }

    return(
        <div className="form">
            {warningText !== "" ?
                <div className="warning-span-div">
                    <span className="warning-span" style={{ color: "#ff5f4a" }}>{warningText}</span>
                </div> : <></>
            }
            <div className="form-input-div">
                <span>Insira o seu e-mail</span>
                <br />
                <input 
                type="email"
                value={email}
                className="form-input"
                onChange={e => handleEmailChange(e.target.value)} />
            </div>
            <button className="form-button" onClick={() => handleButtonClick()} >Enviar email</button>
            <br />
            <a href={window.location.origin} className="form-link">Voltar ao login</a>
        </div>
    );

    
}