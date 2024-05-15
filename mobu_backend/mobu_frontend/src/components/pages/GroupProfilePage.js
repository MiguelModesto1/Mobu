import React,{ useEffect, useRef, useState } from "react";
import ProfileProperty from "../modular/ProfileProperty";
import Button from "../modular/Button";
import Avatar from "../modular/Avatar";
import GroupMemberItem from "../modular/GroupMemberItem"

/**
 * 
 * Perfil de grupo
 * 
 * @param {*} avatarSrc origem do avatar
 * @param id id do grupo
 * @param isAdmin booleano de administrador de grupo
 * @returns 
 */
export default function GroupProfilePage(){

    const [members, setMembers] = useState([]);
    const [isEditing, setIsEditing] = useState(false);
    const [avatar, setAvatar] = useState("");
    const [groupName, setGroupName] = useState("");
    const [warningText, setWarningText] = useState("");

    const queryStrings = new URLSearchParams(window.location.href);

    const connection = queryStrings.get("connection");
    const id = queryStrings.get("id");
    const isAdmin = queryStrings.get("isAdmin");

    /*const mapMembers = members.map((member) => {
        return (
            <GroupMemberItem
                key={member[0]}
                id={id}
                connection={connection}
                avatar={member[2]}
                personId={member[0]}
                personName={member[1]}
                isAdmin={member[3]}
                isEditing={isEditing}
                onMemberExpeling={handleMemberExpeling} />
        );
    });*/

    const renderResult = useRef(
        <>
            <Avatar avatarProps={{
                src: avatar,
                alt: "avatar do grupo " + groupName,
                size: "100px"
            }} />
            <div className="span-div">
                <span className="group-name-bold">{groupName}</span>
            </div>
            <ProfileProperty
                keyProp={"ID"}
                text={id}
                isEditing={isEditing} />
            <div className="span-div">
                <span className="key-span">Integrantes :</span>
            </div>
            <div className="members-div">
                {/*mapMembers*/}
            </div>
            {isAdmin ? <Button
                text="Editar perfil"
                fromParent="profile-button"
                onClick={handleEditingClick} /> : <></>}
        </>
    );
    const handleMemberExpeling = (member) => {
        let aux = [];
        let decrement = 0;
        for(let i=0; i < members.length; i++){
            if(member !== members[i]){
                aux[i-decrement] = members[i]
            }else{
                decrement--;
            }
        }

        setMembers(aux);
    }

    useEffect(() =>{
        var options={
            method:'GET',
            redirect:'follow'
        }

        const queryParams = `?id=${id}&isGroup=true`

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
            setGroupName(data.groupName);
            setMembers(data.members);
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

                fetch(process.env.REACT_APP_API_URL + "/profile/edit-group-profile", options)
                .then((response) => {
                    if(response.status === 404){
                        setWarningText("Tentativa de edição de perfil inválida");
                    }else{
                        setWarningText("");
                    }
                })
                .catch(err => {console.error("error", err)});

                reader.readAsDataURL(selectedFile);
            }
        }
    }

    function handleEditingSaveClick(){
        postProfile();
        setIsEditing(false);
        renderResult.current = 
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
                isEditing={isEditing} />
                <div className="span-div">
                    <span className="key-span">Integrantes :</span>
                </div>
                <div className="members-div">
                    {/*mapMembers*/}
                </div>
                {isAdmin ? <Button 
                text="Editar perfil"
                fromParent="profile-button"
                onClick={handleEditingClick}/> : <></>}
            </>
        ;
    }

    function handleEditingClick(){
        setIsEditing(true);
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
                    alt:"avatar do grupo " + groupName,
                    size:"100px"
                }} />
                <div className="span-div">
                    <span className="group-name-bold">{groupName}</span>
                </div>
                <ProfileProperty
                keyProp={"Nome de grupo"} 
                text={groupName} 
                isEditing={isEditing} />
                <ProfileProperty 
                keyProp={"Avatar"} 
                text={avatar} 
                isEditing={isEditing} />
                <div className="span-div">
                    <span className="key-span">Integrantes :</span>
                </div>
                <div className="members-div">
                    {/*mapMembers*/}
                </div>
                <Button 
                text="Guardar"
                fromParent="profile-button"
                onClick={handleEditingSaveClick}/>
            </>
        ;
    }

    return(
        <div className="profile">
            {renderResult.current}
        </div>
        
    );

}