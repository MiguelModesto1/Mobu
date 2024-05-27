import React,{ useEffect, useState, useRef } from "react";
import Avatar from "./Avatar";
import TopTextBottomText from "./TopTextBottomText";
import Button from "./Button";

/**
 * 
 * Item do membro do grupo
 *
 * @returns 
 */
export default function GroupMemberItem({ requester, itemId, avatar, personId, personName, isAdmin, isRequesterAdmin, connection, roomId }) {
    //debugger
    /**
     * expulsao de membros do grupo
     * @param {any} member
     */
    const handleMemberExpelling = async (itemId, toUser, roomId) => {
        await connection.invoke("ExpelFromGroup", itemId, toUser, roomId);
    }

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
                top:"#" + personId,
                bottom:personName
                }} />
            {isAdmin ? <span>Admin</span> : <></>}
            {isRequesterAdmin && requester !== personId ? <button onClick={() => handleMemberExpelling(itemId+"", personId+"", roomId+"")}>Expulsar</button> : <></>}
        </div>
    );

}