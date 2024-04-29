import React,{ useState } from "react";
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

    function handleUsernameChange(value){
        setUsername(value);
    }

    function handlePasswordChange(value){
        setPassword(value);
    }

    async function handleButtonClick(){

        var options={
            method: 'POST',
            redirect: 'follow',
            body: JSON.stringify({
                NomeUtilizador: username,
                Password: password
            }).toString(),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            }
        }

        await fetch(process.env.REACT_APP_API_URL + "/login", options)
        .then((response) => {
            if(response.status === 404){
                setWarningText("Tentativa de login inválida!");
            } else {
                return response.json();
            }
        })
        .then(data => window.location.replace("/messages?id=" + data.userId))
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
                <span className="warning-span" color="#ff5f4a">{warningText}</span>
            </div>
            :
            <></>}
            <div className="form-input-div">
                <Input input={{
                    type:"string",
                    title: "Nome de utilizador",
                    value: username,
                    placeholder:""
                }}
                fromParent="form"
                id="username"
                onChange={handleUsernameChange}/>
            </div>
            
            <div className="form-input-div">
                <Input input={{
                    type:"password",
                    title: "Palavra-passe",
                    value: password,
                    placeholder:""
                }}
                fromParent="form"
                id="password"
                onChange={handlePasswordChange}/>
            </div>
            <Button
            text="Log in"
            fromParent="form"
            onClick={handleButtonClick} />
            <div className="links-div">
                <Link linkProps={{
                    href:window.location.origin + "/forgot-password",
                    text:"Esqueci-me da palavra-passe"
                }}
                fromParent="form"/>
                <span>    </span>
                <Link linkProps={{
                    href:window.location.origin + "/register",
                    text:"Não tenho uma conta"
                }}
                fromParent="form"/>
            </div>
        </div>
        </>
        
    );  
}