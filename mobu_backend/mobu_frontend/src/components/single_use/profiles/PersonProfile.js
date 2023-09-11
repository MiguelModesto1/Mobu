import React,{ useEffect, useState } from "react";
import ProfileProperty from "../../modular/ProfileProperty";
import Button from "../../modular/Button";
import Link from "../../modular/Link";
import Avatar from "../../modular/Avatar"

export default function PersonProfile({avatar, texts, isOwner}){

    //TORNAR CODIGO DRY

    const [getTexts, setTexts] = useState({});
    const [getRenderResult, setRenderResult] = useState(
        <>
            <Avatar avatarProps={{
            src:avatar,
            alt:"avatar de " + texts.st,
            size:"300px"
            }} />
            <div className="span-div">
                <span className="group-name-bold">{texts.st}</span>
            </div>
            <ProfileProperty 
            keyProp="ID" 
            text={texts.st} 
            isEditing={false}
            isChangingPassword={false}  />
            <ProfileProperty 
            keyProp="Data de nascimento" 
            text={texts.nd} 
            isEditing={false}
            isChangingPassword={false} />
            <ProfileProperty 
            keyProp="E-mail"
            text={texts.rd} 
            isEditing={false}
            isChangingPassword={false} />
            {isOwner ? <Button 
                text="Editar perfil"
                fromParent="profile-button"
                onClick={handleEditingClick}/> : <></>}
        </>
    );

    /* IMPLEMENTAR BUSCA DE ID, USERNAME, AVATAR, DATA NASCIMENTO E EMAIL */

    function handleEditingSaveClick(){
        setRenderResult(
            <>
                <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + texts.st,
                size:"300px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{texts.st}</span>
                </div>
                <ProfileProperty 
                keyProp="ID"
                text={texts.st} 
                isEditing={false}
                isChangingPassword={false}  />
                <ProfileProperty 
                keyProp="Data de Nascimento" 
                text={texts.nd} 
                isEditing={false}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="E-mail"
                text={texts.rd} 
                isEditing={false}
                isChangingPassword={false} />
                <Button 
                text="Editar perfil"
                fromParent="profile-button"
                onClick={handleEditingClick}/>
            </>
        );
    }

    function handlePasswordSaveClick(){
        setRenderResult(
            <>
                <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + texts.st,
                size:"280px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{texts.st}</span>
                </div>
                <ProfileProperty 
                keyProp="Avatar" 
                text={texts.st} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="Nome de utilizador"
                text={texts.nd} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="Data de nascimento"
                text={texts.rd} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="E-mail" 
                text={texts.th} 
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
        );
        
    }

    function handleEditingClick(){
        setRenderResult(
            <>
                <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + texts.st,
                size:"280px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{texts.st}</span>
                </div>
                <ProfileProperty 
                keyProp="Avatar" 
                text={texts.st} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="Nome de utilzador" 
                text={texts.nd} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="Data de nascimento" 
                text={texts.rd} 
                isEditing={true}
                isChangingPassword={false} />
                <ProfileProperty 
                keyProp="E-mail" 
                text={texts.th} 
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
        );
    }

    function handlePasswordChangeClick(){
        setRenderResult(
            <>
                <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + texts.st,
                size:"300px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{texts.st}</span>
                </div>
                <ProfileProperty 
                keyProp="Introduzir palavra-passe atual"
                text={texts.st} 
                isEditing={false}
                isChangingPassword={true} />
                <ProfileProperty 
                keyProp="Introduzir nova palavra-passe"
                text={texts.nd} 
                isEditing={false}
                isChangingPassword={true} />
                <ProfileProperty 
                keyProp="Repetir nova palavra-passe" 
                text={texts.rd} 
                isEditing={false}
                isChangingPassword={true} />
                <Button 
                text="Guardar"
                fromParent="profile-button"
                onClick={handlePasswordSaveClick}/>
            </>
        );
    }

    return(
        <div className="profile">
            {getRenderResult}
        </div>
        
    );

}