import React,{ useEffect, useState, useRef } from "react";
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
export default function GroupMemberItem({id, connection, avatar, personId, personName, isAdmin, isEditing, onMemberExpeling}){

    const adminLabel = useRef(<></>);
    const button = useRef(<></>);

    useEffect(() => {
        if(isAdmin){

            adminLabel.current = 
                <div
                marginRight={isEditing === false ? "80px" : "50px"} 
                className="group-member-admin-span-div">
                    <span className="group-member-admin-span">Admin</span>
                </div>
            ;

            button.current = isEditing ?
                <Button
                text="Expulsar"
                color="#ff5f4a"
                onClick={() =>{
                    connection.invoke("LeaveGroup", personId + "", id + "");

                    onMemberExpeling([
                        personId,
                        personName,
                        avatar,
                        isAdmin
                    ]);
                    
                }} /> : <></>;
    
        }
    }, [avatar, connection, id, isAdmin, isEditing, onMemberExpeling, personId, personName])

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