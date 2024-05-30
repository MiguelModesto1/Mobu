import React,{ useMemo, useState } from "react";
import Button from "../modular/Button";
import Input from "../modular/Input";
import Avatar from "../modular/Avatar";
import ClickableIcon from "../modular/ClickableIcon";

/**
 * Formulario de fundacao de grupos
 * 
 * @param adminId Id do primeiro administrador (SERA UMA QUERY STRING E NAO UM PROP)
 * @returns 
 */
export default function GroupFoundationForm({adminId}){ 

    const [groupName, setgroupName] = useState("");
    const [avatar, setAvatar] = useState("");
    const [warningText, setWarningText] = useState("");

    const avatarImg = useMemo(() => {
        return <><Avatar avatarProps={{
                    src:{avatar},
                    size:"240px",
                    alt:"Meu avatar"
                }}
                />
                <Input
                input={{
                    title:"",
                    type:"file",
                    value:"",
                    placeholder:""
                }}
                fromParent="hidden-group"
                onChange={handleAvatarSrcChange}/>
                <ClickableIcon
                CIProps={{
                    size:"43px",
                    fill:"none",
                    path:{
                        d:"M32.25 3.58331L39.4166 10.75M3.58331 39.4166L5.8702 31.0314C6.0194 30.4843 6.094 30.2108 6.20852 29.9557C6.3102 29.7292 6.43515 29.5139 6.58134 29.3133C6.74599 29.0873 6.94647 28.8868 7.34743 28.4859L25.8615 9.97183C26.2162 9.61707 26.3936 9.43969 26.5982 9.37322C26.7781 9.31476 26.9719 9.31476 27.1518 9.37322C27.3564 9.43969 27.5337 9.61707 27.8885 9.97183L33.0281 15.1115C33.3829 15.4662 33.5603 15.6436 33.6267 15.8482C33.6852 16.0281 33.6852 16.2219 33.6267 16.4018C33.5603 16.6064 33.3829 16.7837 33.0281 17.1385L14.5141 35.6525C14.1131 36.0535 13.9127 36.254 13.6867 36.4186C13.486 36.5648 13.2707 36.6898 13.0442 36.7914C12.7892 36.906 12.5156 36.9806 11.9686 37.1298L3.58331 39.4166Z",
                        stroke:"2",
                        strokeLinecap:"round",
                        strokeLinejoin:"round"
                    }
                }}
                onIconClick={handleIconClick}
            /></>;
    }, [avatar])

    function handleGroupNameChange(value){
        setgroupName(value);
    }

    function handleAvatarSrcChange(value){
        setAvatar(value);
    }

    function handleIconClick(){
        document.getElementsByClassName('hidden-group-input')[0].click();
    }

    async function handleButtonClick(){
        const fileInput = document.getElementsByClassName("avatar")[0];
        const reader = new FileReader();

        if (fileInput.files.length > 0) {
        const selectedFile = fileInput.files[0];

        reader.onload = async function(event) {

            const base64String = event.target.result;

            var options={
                method: 'POST',
                redirect: 'follow',
                body: JSON.stringify({
                    avatar:base64String,
                    groupName: groupName,
                    admin: adminId
                }),
                headers: {
                    'Content-type': 'application/json; charset=UTF-8'
                }
            }

            await fetch(process.env.REACT_APP_API_URL + "group-foundation", options)
            .then((response) => {
                if(response.status === 201){
                    window.location.assign("/messages")
                }else{
                    setWarningText("Tentativa de formação de grupo inválida");
                }
            })
            .catch(err => {console.error("error", err)});
            };

            reader.readAsDataURL(selectedFile);
        }
    }
        

    return(
        <div className="form">
            {warningText !== "" ?
            <div className="warning-span-div">
                <span className="warning-span" color="#ff5f4a">{warningText}</span>
            </div>
            :
            <></>}
            <div className="avatar-div">
                {avatarImg}
            </div>
            
            <div className="form-input-div">
                <Input input={{
                    type:"text",
                    title:"Nome de grupo",
                    value:"",
                    placeholder:""
                }}
                fromParent="form"
                onChange={handleGroupNameChange}/>
            </div>
            
            <Button
            text="Fundar grupo "
            fromParent="form"
            onClick={handleButtonClick} />
        </div>
    );
}