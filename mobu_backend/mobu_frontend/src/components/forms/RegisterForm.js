import React,{ useEffect, useLayoutEffect, useMemo, useRef, useState } from "react";
import Avatar from "../modular/Avatar";
import ClickableIcon from "../modular/ClickableIcon";

/**
 * Formulario de registo
 * @returns 
 */
export default function RegisterForm(){ 

    const [email, setEmail] = useState("");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [passwordVerf, setPasswordVerf] = useState("");
    const [dataNascimento, setDataNascimento] = useState(new Date())
    const [avatar, setAvatar] = useState(new File([""], ""));
    const [warningText, setWarningText] = useState("");

    const avatarImg = useMemo(() => {
        return <><Avatar avatarProps={{
            src:"./assets/images/default_avatar.png",
            size: "240px",
            alt: "Meu avatar"
        }}
        />
            <span>Avatar</span>
            <br/>
            <input
                type="file"
                className="avatar-input"
                onChange={e => handleAvatarChange(e.target)}
                accept=".jpg,.jpeg,.png"
                name="fotografia" />
            {/*<ClickableIcon
                CIProps={{
                    size: "43px",
                    fill: "none",
                    path: {
                        d: "M32.25 3.58331L39.4166 10.75M3.58331 39.4166L5.8702 31.0314C6.0194 30.4843 6.094 30.2108 6.20852 29.9557C6.3102 29.7292 6.43515 29.5139 6.58134 29.3133C6.74599 29.0873 6.94647 28.8868 7.34743 28.4859L25.8615 9.97183C26.2162 9.61707 26.3936 9.43969 26.5982 9.37322C26.7781 9.31476 26.9719 9.31476 27.1518 9.37322C27.3564 9.43969 27.5337 9.61707 27.8885 9.97183L33.0281 15.1115C33.3829 15.4662 33.5603 15.6436 33.6267 15.8482C33.6852 16.0281 33.6852 16.2219 33.6267 16.4018C33.5603 16.6064 33.3829 16.7837 33.0281 17.1385L14.5141 35.6525C14.1131 36.0535 13.9127 36.254 13.6867 36.4186C13.486 36.5648 13.2707 36.6898 13.0442 36.7914C12.7892 36.906 12.5156 36.9806 11.9686 37.1298L3.58331 39.4166Z",
                        stroke: "2",
                        strokeLinecap: "round",
                        strokeLinejoin: "round"
                    }
                }}
                onIconClick={handleIconClick}
            />*/}</>;
    }, [avatar])

    /**
     * mudanca no estado do nome de utilizador
     */
    function handleUsernameChange(value){
        setUsername(value);
    }

    /**
     * mudanca no estado do email
     */
    function handleEmailChange(value){
        setEmail(value);
    }

    /**
     * mudanca no estado da password
     */
    function handlePasswordChange(value){
        setPassword(value);
    }

    /**
     * mudanca no estado da verificacao da password
     */
    function handlePasswordVerfChange(value){
        setPasswordVerf(value);
    }

    /**
     * mudanca no estado da data de nascimento
     */
    function handleDataNascimentoChange(value) {
        setDataNascimento(value);
    }

    /**
     * mudanca no estado do avatar do utilizador
     */
    function handleAvatarChange(value) {
        displayImage(value)
        setAvatar(value.files[0]);
    }

    /**
     * mostrar imagem introduzida
     */
    function displayImage(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                document.getElementsByClassName('avatar')[0].setAttribute('src', e.target.result);
            };

            reader.readAsDataURL(input.files[0]);
        }
    }

    /** 
     * clique no icone
     */
    function handleIconClick(){
        document.getElementsByClassName('avatar-input')[0].click();
    }

    /**
     * clique no botao de submissao
     */
    async function handleButtonClick(){

        var formData = new FormData();

        formData.append("NomeUtilizador", username);
        formData.append("Email", email);
        formData.append("Password", password);
        formData.append("DataNascimento", dataNascimento);
        formData.append("Avatar", avatar);

        var options={
            method: 'POST',
            body: formData
        }

        if(password === passwordVerf){
            await fetch(process.env.REACT_APP_API_URL + "/register", options)
            .then((response) => {
                if(response.status === 200){
                    window.location.assign("./")
                }else{
                    setWarningText("Tentativa de registo inválida!");
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
                </div> : <></>
            }
            <div className="avatar-div">
                {avatarImg}
            </div>
            
            <div className="form-input-div">
                <span>Nome de utilizador</span>
                <br />
                <input 
                    type = "text"
                    value={username}
                    className="form-input"
                    onChange={e => handleUsernameChange(e.target.value)}
                />
            </div>

            <div className="form-input-div">
                <span>Email</span>
                <br />
                <input
                    type="email"
                    value={email}
                    className="form-input"
                    onChange={e => handleEmailChange(e.target.value)}
                />
            </div>

            <div className="form-input-div">
                <span>Data de nascimento</span>
                <br />
                <input
                    type="datetime-local"
                    value={dataNascimento}
                    className="form-input"
                    onChange={e => handleDataNascimentoChange(e.target.value)}
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
            
            <div className="form-input-div">
                <span>Repetir palavra-passe</span>
                <br />
                <input
                    type="password"
                    value={passwordVerf}
                    className="form-input"
                    onChange={e => handlePasswordVerfChange(e.target.value)}
                />
            </div>

            <button className="form-button" onClick={() => handleButtonClick()}>Registar</button>

            <div className="links-div">
                <a href={window.location.origin} className="form-link">Já tenho conta</a>
            </div>
        </div>
    );
}