import React,{ useState, useEffect } from "react";
import Button from "../../modular/Button";
import Link from "../../modular/Link";
import Input from "../../modular/Input";

/**
 * Formulario de login
 * @returns 
 */
export default function LoginForm(){

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [warningText, setWarningText] = useState("");

    /**
     * mudanca no estado do nome de utilizador
     */
     function handleUsernameChange(value){
        setUsername(value);
    }

    /**
     * mudanca no estado da palavra-passe
     */
    function handlePasswordChange(value){
        setPassword(value);
    }

    /**
     * clique no botao de submissao
     */
    async function handleButtonClick() {
        //debugger;
        var options={
            method: 'POST',
            redirect: 'follow',
            body: JSON.stringify({
                NomeUtilizador: username,
                Password: password
            }).toString(),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            },
            credentials: "include"
        }
        
        await fetch(process.env.REACT_APP_API_URL + "/login", options)
            .then((response) => {
                //debugger;
                if(response.status === 404){
                    setWarningText("Tentativa de login inválida!");
                } else {
                    return response.json();
                }

            })
            .then(data => window.location.assign("/messages?id=" + data.userId))
            .then(data => console.log(data))
            .catch(err => {console.error("error", err)});
    }

    return(
        <>
        <div className="mobu-img-div">
            <img className="mobu-image" src="./assets/images/logo.png" alt="mobu logo" />
        </div>
        <div className="form">
            {warningText !== "" ?
                <div className="warning-span-div">
                    <span className="warning-span" style={{ color:"#ff5f4a"}}>{warningText}</span>
                </div> : <></>
            }
            <div className="form-input-div">
                <span>Nome de utilizador</span>
                <br />
                <input
                    type="text"
                    value={username}
                    className="form-input"
                    onChange={e => handleUsernameChange(e.target.value)}
                />
            </div>
            
            <div className="form-input-div">
                <span>Palavra-passe</span>
                <br />
                <input
                    type="password"
                    value={password}
                    className="form-input"
                    onChange={e => handlePasswordChange(e.target.value)}
                />
            </div>

            <button className="form-button" onClick={() => handleButtonClick()}>Log in</button>
            <div className="links-div">
                <a href={window.location.origin + "/forgot-password"} className="form-link">Esqueci-me da palavra-passe</a>
                <span>    </span>
                <a href={window.location.origin + "/register"} className="form-link">Não tenho uma conta</a>
            </div>
        </div>
        </>
        
    );  
}