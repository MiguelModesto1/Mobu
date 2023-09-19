import React,{ useEffect, useState } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";
import Button from "./Button";

/**
 * 
 * Item do membro do grupo
 * 
 * @param {*} avatar origem do avatar
 * @param {*} personId id do utilizador
 * @param personName nome do utilizador
 * @param isAdmin booleano de administrador
 * @param isEditing booleano de edicao
 * @returns 
 */
export default function GroupMemberItem({avatar, personId, personName, isAdmin, isEditing}){

    const [expel, setExpel] = useState(false);
    const [adminLabel, setAdminLabel] = useState(<></>);
    const [button, setButton] = useState(<></>);

    function handleExpelButtonClick(){
        if(expel){
            setExpel(true);
        }
    }

    useEffect(() => {
        if(isAdmin){

            setAdminLabel(
                <div
                marginRight={isEditing === false ? "80px" : "50px"} 
                className="group-member-admin-span-div">
                    <span className="group-member-admin-span">Admin</span>
                </div>
            );

            setButton(isEditing ?
                <Button
                text="Expulsar"
                color="#3b9ae1"
                onClick={handleExpelButtonClick} /> : <></>);
    
        }
    }, [])

    return(
        <div className="person-group-found-item-div">
            <Avatar avatarProps={{
                src:avatar,
                alt:"avatar de " + personName,
                size:"40px"
            }}/>
            <TopTextBottomText 
            marginRight={isAdmin === false ? "375px" : "290px"}
            TTBTProps={{
                top:personId,
                bottom:personName
            }}/>
            {adminLabel}
            {button}
        </div>
    );

}