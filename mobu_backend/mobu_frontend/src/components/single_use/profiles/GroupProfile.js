import React,{ useState } from "react";
import ProfileProperty from "../../modular/ProfileProperty";
import Button from "../../modular/Button";
import Avatar from "../../modular/Avatar"

export default function GroupProfileProfile({avatar, texts, isAdmin}){

    //TORNAR CODIGO DRY

    const [getTexts, setTexts] = useState({});
    const [getRenderResult, setRenderResult] = useState(
        <>
            <Avatar avatarProps={{
                src:avatar,
                alt:"avatar do grupo " + texts.nd,
                size:"100px"
            }} />
            <div className="span-div">
                <span className="group-name-bold">{texts.st}</span>
            </div>
            <ProfileProperty 
            keyProp={"ID"} 
            text={texts.st} 
            isEditing={false} />
            <div className="span-div">
                <span className="key-span">Integrantes :</span>
            </div>
            <div className="members-div">
                {/* IMPLEMENTAR MAPEAMENTO DE MEMBROS */}
            </div>
            {isAdmin ? <Button 
            text="Editar perfil"
            fromParent="profile-button"
            onClick={handleEditingClick}/> : <></>}
        </>
    );

    /* IMPLEMENTAR BUSCA DE NOME DE GRUPO, ID E INTEGRANTES */

    function handleEditingSaveClick(){
        setRenderResult(
            <>
                <Avatar avatarProps={{
                    src:avatar,
                    alt:"avatar do grupo " + texts.nd,
                    size:"100px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{texts.st}</span>
                </div>
                <ProfileProperty 
                keyProp={"ID"} 
                text={texts.st} 
                isEditing={false} />
                <div className="span-div">
                    <span className="key-span">Integrantes :</span>
                </div>
                <div className="members-div">
                    {/* IMPLEMENTAR MAPEAMENTO DE MEMBROS */}
                </div>
                {isAdmin ? <Button 
                text="Editar perfil"
                fromParent="profile-button"
                onClick={handleEditingClick}/> : <></>}
            </>
        );
    }

    function handleEditingClick(){
        setRenderResult(
            <>
                <Avatar avatarProps={{
                    src:avatar,
                    alt:"avatar do grupo " + texts.nd,
                    size:"100px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{texts.st}</span>
                </div>
                <ProfileProperty
                keyProp={"Nome de grupo"} 
                text={texts.st} 
                isEditing={true} />
                <ProfileProperty 
                keyProp={"Avatar"} 
                text={texts.nd} 
                isEditing={true} />
                <div className="span-div">
                    <span className="key-span">Integrantes :</span>
                </div>
                <div className="members-div">
                    {/* IMPLEMENTAR MAPEAMENTO DE MEMBROS C/BOTAO DE EXPULSAO */}
                </div>
                <Button 
                text="Guardar"
                fromParent="profile-button"
                onClick={handleEditingSaveClick}/>
            </>
        );
    }

    return(
        <div className="profile">
            {getRenderResult}
        </div>
        
    );

}