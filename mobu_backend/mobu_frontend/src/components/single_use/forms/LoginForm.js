import React,{ useState } from "react";
import Button from "../../modular/Button";
import Link from "../../modular/Link";
import Input from "../../modular/Input";

/**
 * Formulario de login
 * @returns 
 */
export default function LoginForm(){

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [warningText, setWarningText] = useState("");

    function handleEmailChange(value){
        setEmail(value);
    }

    function handlePasswordChange(value){
        setPassword(value);
    }

    async function handleButtonClick(){

        var options={
            method: 'POST',
            redirect: 'follow',
            body: JSON.stringify({
                email: email,
                password: password
            }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8'
            }
        }

        await fetch(process.env.REACT_APP_API_URL + "/login", options)
        .then((response) => {
            if(response.status === 404){
                setWarningText("Tentativa de login inválida");
            }else{
                window.location.assign("./messages?email=" + email + "&id=0")
            }
        })
        .then((status) => {
            
        })
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
                type:"email",
                title:"E-mail",
                value:"",
                placeholder:""
                }}
                fromParent="form"
                id="email"
                onChange={handleEmailChange}/>
            </div>
            
            <div className="form-input-div">
                <Input input={{
                    type:"password",
                    title:"Palavra-passe",
                    value:"",
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
                <span>    </span>
                <Link 
                linkProps={{
                    href: "https://localhost:7273",
                    text:"Sou administrador"
                }}
                fromParent="form" />
            </div>
        </div>
        </>
        
    );  
}