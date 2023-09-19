import React,{ useEffect, useState } from "react";
import ProfileProperty from "../../modular/ProfileProperty";
import Button from "../../modular/Button";
import Avatar from "../../modular/Avatar"

/**
 * 
 * Perfil de grupo
 * 
 * @param {*} avatarSrc origem do avatar 
 * @param id id do grupo
 * @param isAdmin booleano de administrador de grupo
 * @returns 
 */
export default function GroupProfile({avatarSrc, id, isAdmin}){

    //TORNAR CODIGO DRY

    const [avatar, setAvatar] = useState("");
    const [groupName, setGroupName] = useState("");
    const [warningText, setWarningText] = useState("");
    const [renderResult, setRenderResult] = useState(
        <>
            <Avatar avatarProps={{
                src:avatarSrc,
                alt:"avatar do grupo " + groupName,
                size:"100px"
            }} />
            <div className="span-div">
                <span className="group-name-bold">{groupName}</span>
            </div>
            <ProfileProperty 
            keyProp={"ID"} 
            text={id} 
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

    useEffect(() =>{
        var options={
            method:'GET',
            redirect:'follow'
        }

        const queryParams = `?id=${id}&isGroup=true`

        fetch(process.env.REACT_APP_API_URL + "profile/get-profile" + queryParams, options)
        .then((response) => [response.json(), response.status])
        .then((data) => {
                if(data[1] === 204){
                    setAvatar(data[0].avatar);
                    setGroupName(data[0].groupName);
                }else{
                    setWarningText("Perfil inválido ou inexistente!")
                }
        })
        .catch((err) => {console.error("error", err)});
    }, [id]);

    const postProfile = () =>{
        
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
                        groupName:groupName
                    }),
                    headers: {
                        'Content-type': 'application/json; charset=UTF-8'
                    }
                }

                fetch(process.env.REACT_APP_API_URL + "profile/edit-group-profile", options)
                .then((response) => response.status)
                .then((status) => {
                    setWarningText(status === 404 ? "Tentativa de edição de perfil inválida" : "");
                })
                .catch(err => {console.error("error", err)});

                reader.readAsDataURL(selectedFile);
            }
        }
    }

    /* IMPLEMENTAR BUSCA DE NOME DE GRUPO, ID E INTEGRANTES */

    function handleEditingSaveClick(){
        postProfile();
        setRenderResult(
            <>
                <Avatar avatarProps={{
                    src:avatar,
                    alt:"avatar do grupo " + groupName,
                    size:"100px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{groupName}</span>
                </div>
                <ProfileProperty 
                keyProp={"ID"} 
                text={id} 
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
                {warningText !== "" ?
                <div className="warning-span-div">
                    <span className="warning-span" color="#ff5f4a">{warningText}</span>
                </div>
                :
                <></>}
                <Avatar avatarProps={{
                    src:avatar,
                    alt:"avatar do grupo " + groupName,
                    size:"100px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{groupName}</span>
                </div>
                <ProfileProperty
                keyProp={"Nome de grupo"} 
                text={groupName} 
                isEditing={true} />
                <ProfileProperty 
                keyProp={"Avatar"} 
                text={avatar} 
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
            {renderResult}
        </div>
        
    );

}