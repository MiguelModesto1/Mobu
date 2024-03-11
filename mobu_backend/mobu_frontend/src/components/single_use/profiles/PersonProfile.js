import React,{ useEffect, useRef, useState } from "react";
import ProfileProperty from "../../modular/ProfileProperty";
import Button from "../../modular/Button";
import Link from "../../modular/Link";
import Avatar from "../../modular/Avatar"

/**
 * 
 * Perfil individual
 * 
 * @returns 
 */
export default function PersonProfile(){

    const queryStrings = new URLSearchParams(window.location.href);

    const id = queryStrings.get("id");
    const isOwner = queryStrings.get("isOwner");

    const [username, setUsername] = useState("");
    const [birthDate, setBirthDate] = useState("");
    const [email, setEmail] = useState("");
    const [avatar , setAvatar] = useState("");
    const [currPassword, setCurrPassword] = useState("");
    const [newPassword, setNewPassword] = useState("");
    const [passwordVerf, setPasswordVerf] = useState("");
    const [warningText, setWarningText] = useState("");

    const renderResult = useRef(
        <>
            {warningText !== "" ?
            <div className="warning-span-div">
                <span className="warning-span" color="#ff5f4a">{warningText}</span>
            </div>
            :
            <></>}
            <Avatar avatarProps={{
            src:avatar,
            alt:"avatar de " + id,
            size:"300px"
            }} />
            <div className="span-div">
                <span className="group-name-bold">{username}</span>
            </div>
            <ProfileProperty 
            keyProp="ID" 
            text={id} 
            isEditing={false}
            isChangingPassword={false}  />
            <ProfileProperty 
            keyProp="Data de nascimento" 
            text={birthDate} 
            isEditing={false}
            isChangingPassword={false} />
            <ProfileProperty 
            keyProp="E-mail"
            text={email} 
            isEditing={false}
            isChangingPassword={false} />
            {isOwner ? <Button 
                text="Editar perfil"
                fromParent="profile-button"
                onClick={handleEditingClick}/> : <></>}
        </>
    );

    useEffect(() =>{
        var options={
            method:'GET',
            redirect:'follow'
        }

        const queryParams = `?id=${id}&isGroup=false`

        fetch(process.env.REACT_APP_API_URL + "/profile/get-profile" + queryParams, options)
        .then((response) => {
            if(response.status === 204){
                return response.json();
            }else{
                setWarningText("Perfil inválido ou inexistente!")
            }
        })
        .then((data) => {
            setAvatar(data.avatar);
            setUsername(data.username);
            setEmail(data.email);
            setBirthDate(data.birthDate);
        })
        .catch((err) => {console.error("error", err)});
    },[id]);

    const postProfile = async () =>{

        const fileInput = document.getElementsByClassName("avatar")[0];
        const reader = new FileReader();

        if (fileInput.files.length > 0) {
            const selectedFile = fileInput.files[0];

            reader.onload = async function(event) {

                const base64String = event.target.result;

                var options={
                    method:'POST',
                    redirect:'follow',
                    body: JSON.stringify({
                        id:id,
                        avatar:base64String,
                        username:username,
                        newPassword:newPassword,
                        currPassword:currPassword,
                        birthDate:birthDate
                    }),
                    headers: {
                        'Content-type': 'application/json; charset=UTF-8'
                    }
                }
                if(newPassword === passwordVerf){
                    fetch(process.env.REACT_APP_API_URL + "/profile/edit-person-profile", options)
                    .then((response) => {
                        setWarningText(response.status === 404 ? "Tentativa de edição de perfil inválida" : "");
                    })
                    .catch(err => {console.error("error", err)});
                }else{
                    setWarningText("Passwords têm que conicidir!");
                }
                
                reader.readAsDataURL(selectedFile);
            }
        }
    }

    /* IMPLEMENTAR BUSCA DE ID, USERNAME, AVATAR, DATA NASCIMENTO E EMAIL */

    function handleEditingSaveClick(){
        postProfile();
        renderResult.current = 
            <>
                {warningText !== "" ?
                <div className="warning-span-div">
                    <span className="warning-span" color="#ff5f4a">{warningText}</span>
                </div>
                :
                <></>}
                <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + username,
                size:"300px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{username}</span>
                </div>
                <ProfileProperty 
                keyProp="ID"
                text={id} 
                isEditing={false}
                isChangingPassword={false}  />
                <ProfileProperty 
                keyProp="Data de Nascimento" 
                text={birthDate} 
                isEditing={false}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="E-mail"
                text={email} 
                isEditing={false}
                isChangingPassword={false} />
                <Button 
                text="Editar perfil"
                fromParent="profile-button"
                onClick={handleEditingClick}/>
            </>
        ;
    }

    function handlePasswordSaveClick() {
        renderResult.current = 
            <>
                {warningText !== "" ?
                <div className="warning-span-div">
                    <span className="warning-span" color="#ff5f4a">{warningText}</span>
                </div>
                :
                <></>}
                <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + username,
                size:"280px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{username}</span>
                </div>
                <ProfileProperty 
                keyProp="Avatar" 
                text={avatar} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="Nome de utilizador"
                text={username} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="Data de nascimento"
                text={birthDate} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="E-mail" 
                text={email} 
                isEditing={true}
                isChangingPassword={false} />
                <div className="profile-link-div">
                    <Link 
                    linkProps={{
                        href:"",
                        text:"Mudar palavra-passe"
                    }}
                    onClick={handlePasswordChangeClick} />
                </div>
                <Button 
                text="Guardar"
                fromParent="profile-button"
                onClick={handleEditingSaveClick}/>
            </>
        ;
        
    }

    function handleEditingClick(){
        renderResult.current = 
            <>
                {warningText !== "" ?
                <div className="warning-span-div">
                    <span className="warning-span" color="#ff5f4a">{warningText}</span>
                </div>
                :
                <></>}
                <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + username,
                size:"280px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{username}</span>
                </div>
                <ProfileProperty 
                keyProp="Avatar" 
                text={avatar} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="Nome de utilizador" 
                text={username} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="Data de nascimento" 
                text={birthDate} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="E-mail" 
                text={email} 
                isEditing={true}
                isChangingPassword={false} />
                <div className="profile-link-div">
                    <Link 
                    linkProps={{
                        href:"",
                        text:"Mudar palavra-passe"
                    }}
                    onClick={handlePasswordChangeClick} />
                </div>
                <Button 
                text="Guardar"
                fromParent="profile-button"
                onClick={handleEditingSaveClick}/>
            </>
        ;
    }

    function handlePasswordChange(value){
        setNewPassword(value);
    }

    function handleCurrPasswordChange(value){
        setCurrPassword(value);
    }

    function handlePasswordVerfChange(value){
        setPasswordVerf(value);
    }

    function handlePasswordChangeClick(){
        renderResult.current = 
            <>
                {warningText !== "" ?
                <div className="warning-span-div">
                    <span className="warning-span" color="#ff5f4a">{warningText}</span>
                </div>
                :
                <></>}
                <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + username,
                size:"300px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{username}</span>
                </div>
                <ProfileProperty 
                keyProp="Introduzir palavra-passe atual"
                text="" 
                isEditing={false}
                isChangingPassword={true}
                onChangeText={handleCurrPasswordChange} />
                <ProfileProperty 
                keyProp="Introduzir nova palavra-passe"
                text=""
                isEditing={false}
                isChangingPassword={true}
                onChangeText={handlePasswordChange} />
                <ProfileProperty 
                keyProp="Repetir nova palavra-passe" 
                text="" 
                isEditing={false}
                isChangingPassword={true}
                onChangeText={handlePasswordVerfChange} />
                <Button 
                text="Guardar"
                fromParent="profile-button"
                onClick={handlePasswordSaveClick}/>
            </>
        ;
    }

    return(
        <div className="profile">
            {renderResult}
        </div>
        
    );

}